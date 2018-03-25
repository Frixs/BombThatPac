using Managers;
using UnityEditor.Graphs.AnimationBlendTree;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Characters
{
	public abstract class Ghost : ArtificialIntelligence
	{
		public override float RespawnDeathDelay { get; set; } = Constants.GhostRespawnDeathDelay;
		
		/// <summary>
		/// The Ghost's movement speed.
		/// </summary>
		protected override float Speed { get; set; } = Constants.GhostDefaultSpeed;

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
		
		/// <summary>
		/// Current direction with memory of the last other direction.
		/// </summary>
		private Vector2 _direction, _previousDirection;
		
		// Use this for initialization
		protected override void Start()
		{
			// Get position of the ghost.
			Vector3Int cell = MapManager.Instance.TilemapGameplay.WorldToCell(transform.localPosition);
			Vector3 cellCenter = MapManager.Instance.TilemapGameplay.GetCellCenterWorld(cell);
			// Set current position of the ghost.
			if (cellCenter != Vector3.zero)
			{
				_currentCell = cellCenter;
			}
			// Set initial direction.
			_direction = Vector2.up;
			// Set previous direction as current for default.
			_previousCell = _currentCell;
			// Find first target position.
			_targetCell = ChooseNextCell();
			
			base.Start();
		}
	
		// Update is called once per frame
		protected override void Update()
		{
			ModeUpdate();

			base.Update();
		}

		/// <summary>
		/// Kill the character.
		/// </summary>
		/// <param name="attacker">Reference to attacker character.</param>
		public override void Kill(Character attacker)
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Immediately kills the character without any reason.
		/// </summary>
		/// <param name="respawn">TRUE for respawn after death. FALSE for no respawn anymore.</param>
		public override void ForceKill(bool respawn)
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// Move in the correct direction.
		/// </summary>
		public override void Move()
		{
			/*
			Vector3Int cell = MapManager.Instance.TilemapGameplay.WorldToCell(transform.position);
			Vector3 cellCenterPos = MapManager.Instance.TilemapGameplay.GetCellCenterWorld(cell);
			Vector3Int originCell = MapManager.Instance.TilemapGameplay.WorldToCell(cellCenterPos);
			
			TileBase tile = MapManager.Instance.TilemapGameplay.GetTile<TileBase>(cell);
			if (tile == MapManager.Instance.WallTile)
				return;
			*/

			if (_currentCell != _targetCell && _targetCell != Vector3.zero)
			{
				if (OverShotTarget())
				{
					_currentCell = _targetCell;

					transform.localPosition = _currentCell;
					
					// portal

					_targetCell = ChooseNextCell();
					_previousCell = _currentCell;
					_currentCell = Vector3.zero;
				}
				else
				{
					transform.position += (Vector3) _direction * Speed * Time.deltaTime;
				}
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
		/// Chose next direction tile of ghost's move.
		/// </summary>
		/// <returns>Target position of target tile.</returns>
		private Vector3 ChooseNextCell()
		{
			float[,] cellNeighborDirections = {
				// x,   y,  z
				{ 1f,  0f, 0f}, // Right 
				{ 0f,  1f, 0f}, // Up
				{-1f,  0f, 0f}, // Left
				{ 0f, -1f, 0f}, // Down
			};
			
			// Get player's tile position.
			Vector3Int target = MapManager.Instance.TilemapGameplay.WorldToCell(GetClosestPlayer().transform.position);
			Vector3 targetCenter = MapManager.Instance.TilemapGameplay.GetCellCenterWorld(target);

			// Target position of the next move.
			Vector3 moveToCell = Vector3.zero;

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
				if (tile != MapManager.Instance.WallTile)
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
				_direction = foundCellsDirection[0];
			}
			else if (cellCounter > 1)
			{
				float shortestDistance = float.MaxValue;

				for (int i = 0; i < foundCells.Length; i++)
				{
					if (foundCellsDirection[i] != Vector2.zero)
					{
						float distance = Vector3.Distance(foundCells[i], targetCenter);

						if (distance < shortestDistance)
						{
							shortestDistance = distance;
							moveToCell = foundCells[i];
							_direction = foundCellsDirection[i];
						}
					}
				}
			}

			return moveToCell;
		}

		/// <summary>
		/// Get the closest player from the ghost.
		/// </summary>
		/// <returns>Player script reference.</returns>
		private Player GetClosestPlayer()
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
	}
}
