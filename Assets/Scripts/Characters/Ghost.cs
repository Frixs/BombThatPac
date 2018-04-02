using System;
using Managers;
using StatusEffects.Scriptable;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Characters
{
	[Serializable]
	public abstract class Ghost : ArtificialIntelligence
	{
		public override float RespawnDeathDelay { get; set; } = Constants.GhostRespawnDeathDelay;
		
		/// <summary>
		/// Timer to release ghosts from the start.
		/// </summary>
		protected abstract float GhostReleaseTimer { get; set; }
		
		public override float MoveSpeed { get; set; } = Constants.GhostDefaultMoveSpeed;

		/// <summary>
		/// Spawn position of the ghost.
		/// </summary>
		public abstract Transform SpawnPosition { get; set; }
		
		/// <summary>
		/// Ghost scatter base position.
		/// </summary>
		public abstract Transform ScatterBasePosition { get; set; }
		
		/// <summary>
		/// First target position till leaving the ghost house.
		/// </summary>
		public abstract Transform StartTargetPosition { get; set; }
		
		/// <summary>
		/// Reference to frightened animation controller.
		/// </summary>
		protected RuntimeAnimatorController AnimationControllerFrightenedBlue;
		
		/// <summary>
		/// Reference to frightened animation controller.
		/// </summary>
		protected RuntimeAnimatorController AnimationControllerFrightenedWhite;

		/// <summary>
		/// Sprite for eyes texture when ghost is death and it is moving back to its ghost house.
		/// </summary>
		[Header("Death Eyes Sprites")][SerializeField] protected Sprite SpriteEyesUp;
		/// <summary>
		/// Sprite for eyes texture when ghost is death and it is moving back to its ghost house.
		/// </summary>
		[SerializeField] protected Sprite SpriteEyesLeft;
		/// <summary>
		/// Sprite for eyes texture when ghost is death and it is moving back to its ghost house.
		/// </summary>
		[SerializeField] protected Sprite SpriteEyesDown;
		/// <summary>
		/// Sprite for eyes texture when ghost is death and it is moving back to its ghost house.
		/// </summary>
		[SerializeField] protected Sprite SpriteEyesRight;

		/// <summary>
		/// Checks if ghost is still in ghost house.
		/// </summary>
		[Header("Settings")] public bool IsInGhostHouse;
		
		/// <summary>
		/// Default value of 'IsInGhostHouse' set in inicialization.
		/// </summary>
		private bool _isInGhostHouseDefaultVal;
		
		/// <summary>
		/// Default mode start iteration.
		/// </summary>
		private int _modeChangeIteration = 1;
		
		/// <summary>
		/// Timer for changing between modes.
		/// </summary>
		private float _modeChangeTimer = 0;

		/// <summary>
		/// Timer for changing back to normal/Chase mode.
		/// </summary>
		private float _frightenedModeTimer = 0;
		
		/// <summary>
		/// Timer for blinking in frightened mode.
		/// </summary>
		private float _frightenedBlinkTimer = 0;
		
		/// <summary>
		/// Check if ghost is white (blinking) in frightened mode.
		/// </summary>
		private bool _frightenedModeIsWhite = false;

		/// <summary>
		/// All mode types.
		/// </summary>
		public enum Mode
		{
			Chase,
			Scatter,
			Frightened,
			Consumed
		}

		/// <summary>
		/// Current ghost's mode.
		/// </summary>
		public Mode CurrentMode { get; protected set; } = Mode.Scatter;
		
		/// <summary>
		/// Previous ghost's mode.
		/// </summary>
		private Mode _previousMode;

		/// <summary>
		/// Target location is cell position 1 ahead, Previous location is the location from which the ghost just left. Current is auxiliary variable.
		/// </summary>
		private Vector3 _currentCell, _targetCell, _previousCell;
		
		/// <summary>
		/// TODO
		/// </summary>
		[Header("Status Effects")] [SerializeField] private ScriptableStatusEffect _frightenedMoveSpeedStatusEffect;
		
		/// <summary>
		/// TODO
		/// </summary>
		[SerializeField] private ScriptableStatusEffect _consumedMoveSpeedStatusEffect;

		// TODO Remove these 3 variables after making animations for the ghosts.
		private Sprite _defaultSprite;
		public Sprite FrightenedBlueSprite;
		public Sprite FrightenedWhiteSprite;
		
		// Use this for initialization
		protected override void Start()
		{
			// TODO Remove this line after making animations for the ghosts.
			_defaultSprite = GetComponent<SpriteRenderer>().sprite;
			
			HasEnabledActions = false;
			_isInGhostHouseDefaultVal = IsInGhostHouse;
			
			// Get position of the ghost.
			Vector3Int cell = MapManager.Instance.TilemapGameplay.WorldToCell(transform.localPosition);
			Vector3 cellCenter = MapManager.Instance.TilemapGameplay.GetCellCenterWorld(cell);
			// Set current position of the ghost.
			_currentCell = cellCenter;

			// Set default direction.
			Direction = PreviousDirection = Vector2.left;
			
			// Set initial direction and find first target position.
			_targetCell = ChooseNextCell();

			// Set previous direction as current for default.
			_previousCell = _currentCell;
			
			base.Start();
		}
	
		// Update is called once per frame
		protected override void Update()
		{
			ModeUpdate();

			base.Update();
		}
		
		// Fixed update
		protected override void FixedUpdate()
		{
			ReleaseGhost();
			
			base.FixedUpdate();
		}
		
		protected override void HandleAnimationLayers()
		{
			// TODO Add animations and remove static sprites.
			if (CurrentMode != Mode.Frightened && CurrentMode != Mode.Consumed)
			{
				if (MyAnimator.runtimeAnimatorController != AnimationControllerDefault)
					MyAnimator.runtimeAnimatorController = AnimationControllerDefault;

				if (GetComponent<SpriteRenderer>().sprite != _defaultSprite)
					GetComponent<SpriteRenderer>().sprite = _defaultSprite;

				// TODO uncomment this line after adding ghost animations.
				//base.HandleAnimationLayers();
			}
			else if (CurrentMode == Mode.Frightened)
			{
				if (MyAnimator.runtimeAnimatorController != AnimationControllerFrightenedBlue)
					MyAnimator.runtimeAnimatorController = AnimationControllerFrightenedBlue;
				
				if (GetComponent<SpriteRenderer>().sprite != FrightenedBlueSprite && GetComponent<SpriteRenderer>().sprite != FrightenedWhiteSprite)
					GetComponent<SpriteRenderer>().sprite = FrightenedBlueSprite;
			}
			else if (CurrentMode == Mode.Consumed)
			{
				if (MyAnimator.runtimeAnimatorController != null)
					MyAnimator.runtimeAnimatorController = null;

				if (Direction == Vector2.up)
				{
					transform.GetComponent<SpriteRenderer>().sprite = SpriteEyesUp;
				}
				else if (Direction == Vector2.left)
				{
					transform.GetComponent<SpriteRenderer>().sprite = SpriteEyesLeft;
				}
				else if (Direction == Vector2.down)
				{
					transform.GetComponent<SpriteRenderer>().sprite = SpriteEyesDown;
				}
				else if (Direction == Vector2.right)
				{
					transform.GetComponent<SpriteRenderer>().sprite = SpriteEyesRight;
				}
			}
		}

		public override void Kill(Character attacker)
		{
			if (!IsKillable() || !attacker)
				return;

			IsInvulnearable = true;
			CurrentMode = Mode.Consumed; // We are not using ChangeMode method because we don't want to record previous mode.
			StatusEffectManager.Instance.ApplyStatusEffect(this, null, _consumedMoveSpeedStatusEffect);
			
			Debug.unityLogger.LogFormat(LogType.Log, "[{0}] ghost has been killed by character: [{1}]!", Name, attacker.Name);
		}

		public override void ForceKill(bool respawn)
		{
			if (!respawn)
				IsRespawnable = false;
			
			IsInvulnearable = true;
			CurrentMode = Mode.Consumed;
			StatusEffectManager.Instance.ApplyStatusEffect(this, null, _consumedMoveSpeedStatusEffect);
			
			Debug.unityLogger.LogFormat(LogType.Log, "[{0}] ghost has been force killed!", Name);
		}
		
		/// <summary>
		/// OnTrigger event.
		/// </summary>
		/// <param name="other">Reference to item's collider.</param>
		void OnTriggerEnter2D(Collider2D other)
		{
			// Kill the player.
			if (CurrentMode != Mode.Frightened && CurrentMode != Mode.Consumed && other.CompareTag("Player"))
			{
				other.GetComponent<Player>().Kill(this);
			}
		}
		
		/// <summary>
		/// Method to get position of final target which the ghost is searching for.
		/// </summary>
		/// <returns>Origin position of the closest player cell.</returns>
		protected abstract Vector3Int GetCellOfTarget();

		public override void Move()
		{
			if (_currentCell == _targetCell)
				return;
			
			if (OverShotTarget())
			{
				_currentCell = _targetCell;

				transform.localPosition = _currentCell;
					
				// portal code
				
				// When the ghost reach its spawn point after death, lets put him back to its normal mode to continue.
				if (CurrentMode == Mode.Consumed && _currentCell == SpawnPosition.position)
				{
					IsInvulnearable = false;
					
					StatusEffectManager.Instance.RemoveStatusEffect(this, null, _consumedMoveSpeedStatusEffect, false, StatusEffectManager.RemoveMethod.RemoveTheFirst);
					
					if (!IsRespawnable)
						Destroy(this);
					
					ChangeMode(_previousMode);
					IsInGhostHouse = _isInGhostHouseDefaultVal;
				}

				PreviousDirection = Direction;
				_targetCell = ChooseNextCell();
				_previousCell = _currentCell;
				_currentCell = Vector3.zero;
			}
			else
			{
				transform.position += (Vector3) Direction * MoveSpeed * Time.deltaTime * (HasEnabledActions ? 1 : 0);
			}
		}
		
		/// <summary>
		/// Change mode depending on current game time.
		/// </summary>
		private void ModeUpdate()
		{
			if (CurrentMode != Mode.Frightened)
			{
				// Timer works only if ghost are out of frightened mode.
				_modeChangeTimer += Time.deltaTime;

				// Go through all mode iterations and switch mode to correct one if time of the mode expires.
				for (int i = 1; i <= Constants.GhostModeNumberOfIterations; i++)
				{
					if (_modeChangeIteration != i)
						continue;
					
					if (CurrentMode == Mode.Scatter && _modeChangeTimer > Constants.GhostScatterModeTimer[i - 1])
					{
						ChangeMode(Mode.Chase);
						_modeChangeTimer = 0;
					}

					if (_modeChangeIteration != Constants.GhostModeNumberOfIterations && CurrentMode == Mode.Chase && _modeChangeTimer > Constants.GhostChaseModeTimer[i - 1])
					{
						_modeChangeIteration = i + 1;
						ChangeMode(Mode.Scatter);
						_modeChangeTimer = 0;
					}
				}
			}
			else if (CurrentMode == Mode.Frightened)
			{
				_frightenedModeTimer += Time.deltaTime;

				// Frightened timer.
				if (_frightenedModeTimer >= Constants.GhostFrightenedModeDuration)
				{
					_frightenedModeTimer = 0f;
					ChangeMode(_previousMode);
				}
				// Blinking in frightened mode.
				else if (_frightenedModeTimer >= Constants.GhostFrightenedModeStartBlinkingAt)
				{
					_frightenedBlinkTimer += Time.deltaTime;

					if (_frightenedBlinkTimer >= Constants.GhostFrightenedModeBlinkingSpeed)
					{
						_frightenedBlinkTimer = 0f;
						
						if (_frightenedModeIsWhite) // Go to blue.
						{
							MyAnimator.runtimeAnimatorController = AnimationControllerFrightenedBlue;
							_frightenedModeIsWhite = false;
							
							// TODO remove this line after adding animations.
							GetComponent<SpriteRenderer>().sprite = FrightenedBlueSprite;
						}
						else // Go to white.
						{
							MyAnimator.runtimeAnimatorController = AnimationControllerFrightenedWhite;
							_frightenedModeIsWhite = true;
							
							// TODO remove this line after adding animations.
							GetComponent<SpriteRenderer>().sprite = FrightenedWhiteSprite;
						}
					}
				}
			}
		}
		
		/// <summary>
		/// Change required mode to current mode.
		/// </summary>
		/// <param name="mode">Mode to change.</param>
		private void ChangeMode(Mode mode)
		{
			if (mode == CurrentMode)
				return;
			
			if (CurrentMode == Mode.Frightened)
			{
				StatusEffectManager.Instance.RemoveStatusEffect(this, null, _frightenedMoveSpeedStatusEffect, false, StatusEffectManager.RemoveMethod.RemoveTheFirst);
			}
			
			if (mode == Mode.Frightened)
			{
				StatusEffectManager.Instance.ApplyStatusEffect(this, null, _frightenedMoveSpeedStatusEffect);
			}
			
			_previousMode = CurrentMode;
			CurrentMode = mode;
		}

		/// <summary>
		/// Start frightened mode of the ghost.
		/// </summary>
		public void StartFrightenedMode()
		{
			_frightenedModeTimer = 0f;
			
			if (CurrentMode != Mode.Consumed)
				ChangeMode(Mode.Frightened);
		}
		
		/// <summary>
		/// Starting release ghost timer.
		/// </summary>
		protected void ReleaseGhost()
		{
			if (GhostReleaseTimer < 0f)
				return;
			
			GhostReleaseTimer -= Time.deltaTime;

			if (GhostReleaseTimer > 0f)
				return;

			HasEnabledActions = true;
		}

		/// <summary>
		/// Chose next direction tile of ghost's move.
		/// </summary>
		/// <returns>Target position of target tile.</returns>
		private Vector3 ChooseNextCell()
		{
			float[,] cellNeighborDirections = {
				// x,   y,  z
				{ 0f,  1f, 0f}, // Up
				{-1f,  0f, 0f}, // Left
				{ 0f, -1f, 0f}, // Down
				{ 1f,  0f, 0f}, // Right 
			};
			
			// Get target.
			Vector3 targetCenter = Vector3.zero;
			if (IsInGhostHouse)
			{
				targetCenter = StartTargetPosition.position;
			}
			else if (CurrentMode == Mode.Chase)
			{
				Vector3Int target = GetCellOfTarget();
				targetCenter = MapManager.Instance.TilemapGameplay.GetCellCenterWorld(target);
			}
			else if (CurrentMode == Mode.Scatter)
			{
				targetCenter = ScatterBasePosition.transform.position;
			}
			else if (CurrentMode == Mode.Frightened)
			{
				Vector3Int target = GetRandomCellAroundGhostHouse();
				targetCenter = MapManager.Instance.TilemapGameplay.GetCellCenterWorld(target);
			}
			else if (CurrentMode == Mode.Consumed)
			{
				targetCenter = SpawnPosition.position;
			}
			else
			{
				Debug.unityLogger.Log(LogType.Error, "No target! There is missing definition of the current mode!");
			}

			// Target position of the next move.
			Vector3 moveToCell = Vector3.zero;
			// Direction to moved cell.
			Vector2 direction = Vector2.zero;

			// All neighbors with its directions.
			Vector3[] foundCells = new Vector3[4];
			Vector2[] foundCellsDirection = new Vector2[4];
			
			// Valid neighbor counter.
			int cellCounter = 0;

			// Go through all neighbors and find the valid one.
			for (int i = 0; i < cellNeighborDirections.GetLength(0); i++)
			{
				// Get neighbor tile position in the center.
				Vector3 neighborCenter = _currentCell + new Vector3(cellNeighborDirections[i, 0], cellNeighborDirections[i, 1], cellNeighborDirections[i, 2]);
				// Get tile reference.
				Vector3Int neighbor = MapManager.Instance.TilemapGameplay.WorldToCell(neighborCenter);
				TileBase tile = MapManager.Instance.TilemapGameplay.GetTile<TileBase>(neighbor);
				
				// Find only the valid one.
				// Try to find obstacle in the current cell.
				Collider2D[] obstacles = Physics2D.OverlapCircleAll(
					new Vector2(
						neighborCenter.x,
						neighborCenter.y
					),
					MapManager.Instance.TilemapCellHalfSize,
					1 << LayerMask.NameToLayer(Constants.UserLayerNameObstacle)
				);
				// Evaluate tiles. If it is not wall or obstacle you can go in this direction.
				// If the ghosts are in ghost house (or in Consumed mode), they can go through the obstacle (only obstacle there is door).
				if (tile != MapManager.Instance.WallTile && (obstacles.Length == 0 || (IsInGhostHouse && obstacles.Length > 0) || CurrentMode == Mode.Consumed))
				{
					foundCells[cellCounter] = neighborCenter;
					foundCellsDirection[cellCounter] = new Vector2(cellNeighborDirections[i, 0], cellNeighborDirections[i, 1]);
					cellCounter++;
				}
			}

			// Find the neighbor with shortest straight path to player.
			if (cellCounter == 1)
			{
				moveToCell = foundCells[0];
				direction = foundCellsDirection[0];
			}
			else if (cellCounter > 1)
			{
				float shortestDistance = float.MaxValue;

				for (int i = 0; i < foundCells.Length; i++)
				{
					if (foundCellsDirection[i] != Vector2.zero && IsPossibleToChangeDirection(foundCellsDirection[i], PreviousDirection))
					{
						float distance = Vector3.Distance(foundCells[i], targetCenter);

						if (distance < shortestDistance)
						{
							shortestDistance = distance;
							moveToCell = foundCells[i];
							direction = foundCellsDirection[i];
						}
					}
				}
			}

			Direction = direction;
			return moveToCell;
		}

		/// <summary>
		/// Get the closest player from the ghost.
		/// </summary>
		/// <returns>Player script reference.</returns>
		protected Player GetClosestPlayer()
		{
			Player closestPlayer = null;
			float shortestDistance = float.MaxValue, currentDistance = 0;
			
			for (int i = 0; i < GameManager.Instance.Players.Length; i++)
			{
				currentDistance = Vector3.Distance(gameObject.transform.position, GameManager.Instance.Players[i].Player.transform.position);
				if (currentDistance < shortestDistance)
				{
					closestPlayer = GameManager.Instance.Players[i].Player;
					shortestDistance = currentDistance;
				}
			}

			return closestPlayer;
		}

		/// <summary>
		/// Checks if the ghost already reached the target location (1 cell ahead) or he is still on the way between two cells.
		/// </summary>
		/// <returns>TRUE: The ghost reached the target location.</returns>
		private bool OverShotTarget()
		{
			float cellToTarget = Vector3.Distance(_previousCell, _targetCell);
			float cellToSelf = Vector3.Distance(transform.localPosition, _previousCell);

			return cellToSelf > cellToTarget;
		}

		/// <summary>
		/// Get random cell coords around the start target position.
		/// </summary>
		/// <returns>Random cell around ghost start target position.</returns>
		private Vector3Int GetRandomCellAroundGhostHouse()
		{
			int x = Random.Range(-Constants.GhostGetRandomCellMethodMaxDistance, Constants.GhostGetRandomCellMethodMaxDistance);
			int y = Random.Range(-Constants.GhostGetRandomCellMethodMaxDistance, Constants.GhostGetRandomCellMethodMaxDistance);
			
			Vector3Int startTargetPositionCell = MapManager.Instance.TilemapGameplay.WorldToCell(StartTargetPosition.position);
			
			return new Vector3Int(startTargetPositionCell.x + x, startTargetPositionCell.y + y, 0);
		}

		/// <summary>
		/// Check if it is possible change direction.
		/// F.e. you cannot change direction from LEFT to RIGHT.
		/// </summary>
		/// <param name="direction">Dirrection you wish to change.</param>
		/// <param name="previousDirection">Previous direction.</param>
		/// <returns>TRUE: It is possible direction.</returns>
		private bool IsPossibleToChangeDirection(Vector2 direction, Vector2 previousDirection)
		{
			if (direction == Vector2.left && previousDirection == Vector2.right)
				return false;
			if (direction == Vector2.right && previousDirection == Vector2.left)
				return false;
			if (direction == Vector2.up && previousDirection == Vector2.down)
				return false;
			if (direction == Vector2.down && previousDirection == Vector2.up)
				return false;

			return true;
		}
	}
}
