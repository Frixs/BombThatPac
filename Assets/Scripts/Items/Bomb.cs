using Characters;
using Managers;
using StatusEffects.Scriptable;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Items
{
	public class Bomb : MonoBehaviour
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
		public GameObject ExplosionPrefab;

		/// <summary>
		/// Scriptable asset of for ghost when the bomb hit it it will get debuff.
		/// </summary>
		public ScriptableStatusEffect GhostBombStatusEffect;
		
		/// <summary>
		/// Range when the bomb become collidable after placing it on the ground.
		/// </summary>
		private float _rangeToBecomeCollidable;

		// Use this for initialization
		void Start()
		{
			Countdown = Caster.BombCountdown;
			_rangeToBecomeCollidable = MapManager.Instance.TilemapCellSize / 2f;
			
			// Ignore collision when spawning under your feet.
			Physics2D.IgnoreCollision(Caster.GetComponent<CircleCollider2D>(), GetComponent<CircleCollider2D>());
		}
	
		// Update is called once per frame
		void Update()
		{
			Countdown -= Time.deltaTime;

			if (Countdown <= 0f)
			{
				Explode(transform.position);
				Debug.unityLogger.LogFormat(LogType.Log, "[{0} ({1})] Bomb exploded!", Caster.Identifier, Caster.Name);
				
				Caster.BombDeployCounter--;
				
				Destroy(gameObject);
			}
		}

		// FixedUpdate is called every fixed framerate frame
		void FixedUpdate()
		{
			// Restore the collision when player will be in sufficient distance from the bomb.
			if (Vector2.Distance(Caster.transform.position, transform.position) > _rangeToBecomeCollidable)
			{
				Physics2D.IgnoreCollision(Caster.GetComponent<CircleCollider2D>(), GetComponent<CircleCollider2D>(), false);
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
		}

		/// <summary>
		/// Explosion in certain direction.
		/// </summary>
		/// <param name="origin">Origion of the explosion.</param>
		/// <param name="direction">Direction of explosion - Vector3Int filled only with -1, 0, 1 values. Each direction can has only1 value rest 0. We are on grid only.</param>
		private void ExplodeInDirection(Vector3Int origin, Vector3Int direction)
		{
			for (int i = 1; i <= Caster.BombExplosionDistance; i++)
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

            if (tile == MapManager.Instance.DestructibleTile)
            {
                // Remove the tile.
                MapManager.Instance.TilemapGameplay.SetTile(cell, null);
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

		        if ((component = hitColliders[i].GetComponent<Bomb>()) != null)
		        {
			        ((Bomb) component).Countdown = Constants.BombChainedCountdown;
		        }
                    
		        if ((component = hitColliders[i].GetComponent<Ghost>()) != null)
		        {
			        if (((Ghost) component).CurrentMode == Ghost.Mode.Frightened)
				        ((Ghost) component).Kill(caster);
			        if (((Ghost) component).CurrentMode != Ghost.Mode.Frightened && ((Ghost) component).CurrentMode != Ghost.Mode.Consumed)
			        	StatusEffectManager.Instance.ApplyStatusEffect((Ghost) component, GhostBombStatusEffect.Initialize((Ghost) component));
		        }
	        }

	        // Create an explosion.
            Vector3 pos = MapManager.Instance.TilemapGameplay.GetCellCenterWorld(cell);
            GameObject explosion = (GameObject) Instantiate(ExplosionPrefab, pos, Quaternion.identity);

            // Destroy the explosion after animation.
            Destroy(explosion, ExplosionPrefab.GetComponent<Animator>().runtimeAnimatorController.animationClips.Length);

            return true;
        }
	}
}
