using Characters;
using Managers;
using StatusEffects.Scriptable;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Items
{
	public class ItemBomb : MonoBehaviour
	{
		/// <summary>
		/// Reference to bomb owner.
		/// </summary>
		[HideInInspector] public Player Caster;

		/// <summary>
		/// Bomb's countdown.
		/// </summary>
		[HideInInspector] public float Countdown;
		
		/// <summary>
		/// Reference to explosion PREFAB.
		/// </summary>
		[Header("Settings")] public GameObject ExplosionPrefab;
		
		/// <summary>
		/// Reference to bomb trigger PREFAB.
		/// </summary>
		public GameObject BombTriggerPrefab;

		/// <summary>
		/// Distance of explosion.
		/// </summary>
		[HideInInspector] public int ExplosionDistance;

		/// <summary>
		/// (This distance * chained bomb order) + normal distance of the bomb, if the bomb is in a chain.
		/// </summary>
		public int ChainAdditionalExplosionDistance;
		
		/// <summary>
		/// Scriptable asset of for ghost when the bomb hit it it will get debuff.
		/// </summary>
		[Header("Status Effects")] public ScriptableStatusEffect GhostBombStatusEffect;

		/// <summary>
		/// Sound on explosion.
		/// </summary>
		[Header("Music Settings")] public AudioClip[] ExplosionSfx;
		
		/// <summary>
		/// Check if this bomb is chained or it is the start one.
		/// </summary>
		private bool _isChained = false;

		/// <summary>
		/// Order of the chain. Is this f.e. 3rd bomb in a chain?
		/// </summary>
		private int _chainedOrder = 0;

		/// <summary>
		/// Check if bomb can be rolled.
		/// </summary>
		private bool _rollable;
		
		/// <summary>
		/// Checks if bomb should start rolling. Checks when the bomb has collided with caster. Also it says if the bomb currently rolling or not.
		/// </summary>
		private bool _isRolling;

		/// <summary>
		/// Rolling direction of the bomb. Normalized vector.
		/// </summary>
		private Vector2 _rollingDirection;

		/// <summary>
		/// Position where the bomb has been planted.
		/// </summary>
		private Vector3 _plantedPosition;
		
		/// <summary>
		/// Target location is cell position 1 ahead, Previous location is the location from which the ghost just left. Current is auxiliary variable.
		/// </summary>
		private Vector3 _currentCell, _targetCell, _previousCell;

		// Use this for initialization
		void Start()
		{
			Countdown = Caster.BombCountdown;
			ExplosionDistance = Caster.BombExplosionDistance;
			
			// Rolling default settings.
			_rollable = true;
			_isRolling = false;
			_plantedPosition = transform.position;
			// Get position of the bomb.
			Vector3Int cell = MapManager.Instance.TilemapGameplay.WorldToCell(transform.localPosition);
			Vector3 cellCenter = MapManager.Instance.TilemapGameplay.GetCellCenterWorld(cell);
			// Set current position of the ghost.
			_currentCell = cellCenter;
			// Set previous direction as current for default.
			_previousCell = _currentCell;
			
			// Ignore collision when spawning under your feet.
			Physics2D.IgnoreCollision(Caster.MyCollider, GetComponent<CircleCollider2D>());
			// Set trigger to be able to activate collision on exit.
			ItemBombTrigger trigger = Instantiate(BombTriggerPrefab, transform.position, Quaternion.identity).GetComponent<ItemBombTrigger>();
			trigger.BombCollider = GetComponent<CircleCollider2D>();
			trigger.CharacterCollider = Caster.MyCollider;
			trigger.GetComponent<CircleCollider2D>().offset = GetComponent<CircleCollider2D>().offset;
			trigger.GetComponent<CircleCollider2D>().radius = GetComponent<CircleCollider2D>().radius;
		}
	
		// Update is called once per frame
		void Update()
		{
			Countdown -= Time.deltaTime;
			
			// Roll the bomb if the bomb has been pushed by caster.
			Roll();

			if (Countdown <= 0f && !_isRolling)
			{
				Explode(transform.position);
				Debug.unityLogger.LogFormat(LogType.Log, "[{0}] Bomb exploded!", Caster.Name);
				
				Caster.BombDeployCounter--;
				
				Destroy(gameObject);
			}
			// Dont go under 0, because we want to set explosion delay after rolling.
			else if (Countdown <= 0f)
				Countdown = 0f;
		}

		// FixedUpdate is called every fixed framerate frame
		void FixedUpdate()
		{
		}

		private void OnCollisionEnter2D(Collision2D other)
		{
			Component component = null;
			if ((component = other.gameObject.GetComponent<Player>()) != null && ((Player) component).Identifier == Caster.Identifier)
			{
				// Start rolling the bomb in the correct direction.
				if (!_rollable)
					return;
				
				// Calculate Angle Between the collision point and the player.
				Vector3 dir = other.contacts[0].point - new Vector2(transform.position.x, transform.position.y);
				// We then get the opposite (-Vector3) and normalize it.
				_rollingDirection = -dir.normalized;
				// Normalize the vecor only for 4-side movement.
				if (Mathf.Abs(_rollingDirection.x) > Mathf.Abs(_rollingDirection.y))
					_rollingDirection = new Vector2(_rollingDirection.x, 0f).normalized;
				else
					_rollingDirection = new Vector2(0f, _rollingDirection.y).normalized;
				
				// Set the first target cell to roll there.
				_targetCell = _currentCell + new Vector3(_rollingDirection.x, _rollingDirection.y, 0);
				// Don't try to roll the bomb if the bomb cannot be rolled at all.
				if (!CheckTargetCell())
					return;
				
				_rollable = false;
				_isRolling = true;
			}
				
		}
		
		/// <summary>
		/// Bomb explosion.
		/// </summary>
		/// <param name="worldPos">Position of epicenter.</param>
		public void Explode(Vector2 worldPos)
		{
			Vector3Int originCell = MapManager.Instance.TilemapGameplay.WorldToCell(worldPos);
			
			ExplodeInCell(originCell, Caster);
			
			for (int i = 0; i < Caster.BombExplosionDirection.GetLength(0); i++)
			{
				ExplodeInDirection(originCell, new Vector3Int(Caster.BombExplosionDirection[i, 0], Caster.BombExplosionDirection[i, 1], Caster.BombExplosionDirection[i, 2]));
			}
			
			SoundManager.Instance.PlayRandomizeSfx(ExplosionSfx);
		}

		/// <summary>
		/// Explosion in certain direction.
		/// </summary>
		/// <param name="origin">Origion of the explosion.</param>
		/// <param name="direction">Direction of explosion - Vector3Int filled only with -1, 0, 1 values. Each direction can has only1 value rest 0. We are on grid only.</param>
		private void ExplodeInDirection(Vector3Int origin, Vector3Int direction)
		{
			int explDistance = ExplosionDistance;
			explDistance += _isChained ? _chainedOrder * ChainAdditionalExplosionDistance : 0;
			
			for (int i = 1; i <= explDistance; i++)
			{
				if (!ExplodeInCell(origin + direction * i, Caster))
					break;
			}
		}
		
        /// <summary>
        /// Make explosion in the current cell (by world position pointing to the cell.)
        /// </summary>
        /// <param name="cell">World position of the cell.</param>
        /// <param name="caster">Reference to caster of an effect which caused the explosion.</param>
        /// <returns></returns>
        public bool ExplodeInCell(Vector3Int cell, Character caster)
        {
            TileBase tile = MapManager.Instance.TilemapGameplay.GetTile<TileBase>(cell);
	        Vector3 explosionPos;
	        explosionPos = MapManager.Instance.TilemapGameplay.GetCellCenterWorld(cell);

            // Try to find obstacle in the current cell.
            Collider2D[] obstacles = Physics2D.OverlapCircleAll(
                new Vector2(
                    cell.x + MapManager.Instance.TilemapCellHalfSize,
                    cell.y + MapManager.Instance.TilemapCellHalfSize
                ),
                MapManager.Instance.TilemapCellHalfSize,
                1 << LayerMask.NameToLayer(Constants.UserLayerNameObstacle)
            );

            // End an explosion if explosion wants to hit obstacle.
            if (tile == MapManager.Instance.WallTile || obstacles.Length > 0)
                return false;

	        if (tile == MapManager.Instance.DestructableObstacleTile)
            {
                // Remove the tile.
                MapManager.Instance.TilemapGameplay.SetTile(cell, null);
	            
	            // Create an explosion.
	            SpawnManager.Instance.SpawnAnimationAtPositionWithExpiry(ExplosionPrefab, explosionPos, Quaternion.identity);
	            
                return false;
            }

	        // Find all characters affected by the explosion.
	        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(
		        new Vector2(
			        cell.x + MapManager.Instance.TilemapCellHalfSize,
			        cell.y + MapManager.Instance.TilemapCellHalfSize
		        ),
		        MapManager.Instance.TilemapCellHalfSize,
		        1 << LayerMask.NameToLayer(Constants.UserLayerNameTriggerObject)
	        );

	        // Go through all characters affected by the explosion.
	        for (var i = 0; i < hitColliders.Length; i++)
	        {
		        Component component = null;

		        if ((component = hitColliders[i].GetComponent<Player>()) != null)
		        {
			        ((Player) component).Kill(caster);
		        }

		        if ((component = hitColliders[i].GetComponent<ItemBomb>()) != null && component != this)
		        {
			        ((ItemBomb) component).Countdown = ((ItemBomb) component).Countdown > 0f ? Constants.BombChainedCountdown : ((ItemBomb) component).Countdown;
			        ((ItemBomb) component)._isChained = true;
			        ((ItemBomb) component)._chainedOrder = this._chainedOrder + 1;
		        }
                    
		        if ((component = hitColliders[i].GetComponent<Ghost>()) != null)
		        {
			        if (((Ghost) component).CurrentMode == Ghost.Mode.Frightened)
			        {
				        ((Ghost) component).Kill(caster);
			        }

			        if (((Ghost) component).CurrentMode != Ghost.Mode.Frightened && ((Ghost) component).CurrentMode != Ghost.Mode.Consumed)
			        	StatusEffectManager.Instance.ApplyStatusEffect((Ghost) component, Caster, GhostBombStatusEffect);
		        }
	        }

	        // Create an explosion.
	        SpawnManager.Instance.SpawnAnimationAtPositionWithExpiry(ExplosionPrefab, explosionPos, Quaternion.identity);
	        
            return true;
        }
		
		/// <summary>
		/// Roll the bomb in the correct direction. //Pretty the same method as method Move() in Ghost script.
		/// </summary>
		private void Roll()
		{
			if (!_isRolling)
				return;

			if (_currentCell == _targetCell)
				return;
			
			if (OverShotTarget())
			{
				_currentCell = _targetCell;

				transform.localPosition = _currentCell;
				
				_targetCell = _currentCell + new Vector3(_rollingDirection.x, _rollingDirection.y, 0);

				// Check if thje bomb is on the maximal rolling position or the bomb cannot move towards wall or obstacle.
				if (_currentCell == _plantedPosition + new Vector3(_rollingDirection.x, _rollingDirection.y, 0) * Constants.BombRollDistance || !CheckTargetCell())
				{
					_isRolling = false;
					Countdown += Constants.BombExplosionDelayAfterRolling;
					return;
				}

				_previousCell = _currentCell;
				_currentCell = Vector3.zero;
			}
			else
			{
				transform.position += new Vector3(_rollingDirection.x, _rollingDirection.y, 0) * Constants.BombRollSpeed * Time.deltaTime;
			}
		}

		/// <summary>
		/// Check if target cell is safe to move there.
		/// </summary>
		/// <returns>TRUE: There is NO obstacle or wall or another bomb.</returns>
		private bool CheckTargetCell()
		{
			// Get tile reference.
			Vector3Int neighbor = MapManager.Instance.TilemapGameplay.WorldToCell(_targetCell);
			TileBase tile = MapManager.Instance.TilemapGameplay.GetTile<TileBase>(neighbor);
			
			// Try to find if there is another bomb already planted.
			Collider2D[] triggerObjects = Physics2D.OverlapCircleAll(
				new Vector2(
					_targetCell.x,
					_targetCell.y
				),
				MapManager.Instance.TilemapCellHalfSize,
				1 << LayerMask.NameToLayer(Constants.UserLayerNameTriggerObject)
			);
			for (int i = 0; i < triggerObjects.Length; i++)
				if (triggerObjects[i].CompareTag("Bomb"))
					return false;
			
			// Evaluate tiles. If it is not wall or obstacle you can go in this direction.
			return tile != MapManager.Instance.WallTile && tile != MapManager.Instance.DestructableObstacleTile;
		}
		
		/// <summary>
		/// Checks if the ghost already reached the target location (1 cell ahead) or he is still on the way between two cells. //The same method is in the ghost script.
		/// </summary>
		/// <returns>TRUE: The ghost reached the target location.</returns>
		private bool OverShotTarget()
		{
			float cellToTarget = Vector3.Distance(_previousCell, _targetCell);
			float cellToSelf = Vector3.Distance(transform.localPosition, _previousCell);

			return cellToSelf > cellToTarget;
		}
	}
}
