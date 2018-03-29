using System;
using Managers;
using UnityEditor.Graphs.AnimationBlendTree;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.XR.WSA;

namespace Characters
{
	public abstract class Ghost : ArtificialIntelligence
	{
		public override float RespawnDeathDelay { get; set; } = Constants.GhostRespawnDeathDelay;
		
		/// <summary>
		/// Timer to release ghosts from the start.
		/// </summary>
		protected abstract float GhostReleaseTimer { get; set; }
		
		protected override float Speed { get; set; } = Constants.GhostDefaultSpeed;

		/// <summary>
		/// Spawn position of the ghost. //TODO: generating maze will require GhostSpawnPointManger to handle all these spawn & scatter positions for better organisation.
		/// </summary>
		protected abstract Transform SpawnPosition { get; set; }
		
		/// <summary>
		/// Ghost scatter base position.
		/// </summary>
		protected abstract Transform ScatterBasePosition { get; set; }
		
		/// <summary>
		/// First target position till leaving the ghost house.
		/// </summary>
		protected abstract Transform StartTargetPosition { get; set; }

		/// <summary>
		/// Checks if ghost is still in ghost house.
		/// </summary>
		public bool IsInGhostHouse;
		
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
		/// All mode types.
		/// </summary>
		private enum Mode
		{
			Chase,
			Scatter,
			Frightened
		}

		/// <summary>
		/// Current ghost's mode.
		/// </summary>
		private Mode _currentMode = Mode.Scatter;
		
		/// <summary>
		/// Previous ghost's mode.
		/// </summary>
		private Mode _previousMode;

		/// <summary>
		/// Target location is tile position 1 ahead, Previous location is the location from which the ghost just left. Current is auxiliary variable.
		/// </summary>
		private Vector3 _currentCell, _targetCell, _previousCell;
		
		// Use this for initialization
		protected override void Start()
		{
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

		public override void Kill(Character attacker)
		{
			// TODO
			throw new System.NotImplementedException();
		}

		public override void ForceKill(bool respawn)
		{
			// TODO
			throw new System.NotImplementedException();
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
					
				// portal

				PreviousDirection = Direction;
				_targetCell = ChooseNextCell();
				_previousCell = _currentCell;
				_currentCell = Vector3.zero;
			}
			else
			{
				transform.position += (Vector3) Direction * Speed * Time.deltaTime * (HasEnabledActions ? 1 : 0);
			}
		}
		
		/// <summary>
		/// Change mode depending on current game time.
		/// </summary>
		private void ModeUpdate()
		{
			if (_currentMode != Mode.Frightened)
			{
				// Timer works only if ghost are out of frightened mode.
				_modeChangeTimer += Time.deltaTime;

				// Go through all mode iterations and switch mode to correct one if time of the mode expires.
				for (int i = 1; i <= Constants.GhostModeNumberOfIterations; i++)
				{
					if (_modeChangeIteration != i)
						continue;
					
					if (_currentMode == Mode.Scatter && _modeChangeTimer > Constants.GhostScatterModeTimer[i - 1])
					{
						ChangeMode(Mode.Chase);
						_modeChangeTimer = 0;
					}

					if (_modeChangeIteration != Constants.GhostModeNumberOfIterations && _currentMode == Mode.Chase && _modeChangeTimer > Constants.GhostChaseModeTimer[i - 1])
					{
						_modeChangeIteration = i + 1;
						ChangeMode(Mode.Scatter);
						_modeChangeTimer = 0;
					}
				}
			}
			else
			{
				
			}
		}
		
		/// <summary>
		/// Change required mode to current mode.
		/// </summary>
		/// <param name="mode">Mode to change.</param>
		private void ChangeMode(Mode mode)
		{
			_currentMode = mode;
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
			
			// Get player's tile position.
			Vector3 targetCenter;
			if (IsInGhostHouse)
			{
				targetCenter = StartTargetPosition.position;
			}
			else if (_currentMode == Mode.Scatter)
			{
				targetCenter = ScatterBasePosition.transform.position;
			}
			else // Chase Mode
			{
				Vector3Int target = GetCellOfTarget();
				targetCenter = MapManager.Instance.TilemapGameplay.GetCellCenterWorld(target);
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
				// Evaluate tiles. If it is not wall or obstacle you can go in this direction. If you are in ghost house you can go through the obstacle (only obstacle there is door).
				if (tile != MapManager.Instance.WallTile && (obstacles.Length == 0 || (IsInGhostHouse && obstacles.Length > 0)))
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
		/// Checks if the ghost already reached the target location (1 tile ahead) or he is still on the way between two tiles.
		/// </summary>
		/// <returns>TRUE: The ghost reached the target location.</returns>
		private bool OverShotTarget()
		{
			float cellToTarget = Vector3.Distance(_previousCell, _targetCell);
			float cellToSelf = Vector3.Distance(transform.localPosition, _previousCell);

			return cellToSelf > cellToTarget;
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
