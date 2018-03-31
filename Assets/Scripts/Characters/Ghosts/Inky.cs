using Managers;
using UnityEngine;

namespace Characters
{
	public class Inky : Ghost
	{
		// Use this for initialization
		protected override float GhostReleaseTimer { get; set; } = Constants.GhostInkyReleaseTimer;
		public override Transform SpawnPosition { get; set; }
		public override Transform ScatterBasePosition { get; set; }
		public override Transform StartTargetPosition { get; set; }

		// Awake is always called before any Start functions
		protected void Awake()
		{
		}
		
		protected override void Start ()
		{
			base.Start();
		}
	
		// Update is called once per frame
		protected override void Update ()
		{
			base.Update();
		}

		protected override Vector3Int GetCellOfTarget()
		{
			// Select the position two cells in front of the closest player.
			// Draw Vector from Blinky ghost to that position.
			// X times the length of the vector.
			
			Player closestPlayer = GetClosestPlayer();
			Vector3Int closestPlayerCell = MapManager.Instance.TilemapGameplay.WorldToCell(closestPlayer.transform.position);
			
			Vector2 playerOrientation = closestPlayer.GetOrientation();
			Vector3Int targetCell = closestPlayerCell + new Vector3Int(Mathf.RoundToInt(playerOrientation.x), Mathf.RoundToInt(playerOrientation.y), 0) * Constants.GhostInkyNumberOfCellsAhead;

			Vector3Int blinkyCell = MapManager.Instance.TilemapGameplay.WorldToCell(GameManager.Instance.Ghosts[0].transform.position);
			
			int targetDistanceX = (targetCell.x - blinkyCell.x) * Constants.GhostInkyVectorMultiplier;
			int targetDistanceY = (targetCell.y - blinkyCell.y) * Constants.GhostInkyVectorMultiplier;
			
			return new Vector3Int(targetCell.x + targetDistanceX, targetCell.y + targetDistanceY, targetCell.z);
		}
	}
}
