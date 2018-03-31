using Managers;
using UnityEngine;

namespace Characters
{
	public class Clyde : Ghost
	{
		// Use this for initialization
		protected override float GhostReleaseTimer { get; set; } = Constants.GhostClydeReleaseTimer;
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
			// Calculate the distance from the closest player.
			// If the distance is greater than X cells targeting is the same as Blinky.
			// If the distance is less than X cells, then target is his scatter base, so same as scatter mode.
			
			Player closestPlayer = GetClosestPlayer();
			Vector3Int closestPlayerCell = MapManager.Instance.TilemapGameplay.WorldToCell(closestPlayer.transform.position);

			Vector3Int targetCell = closestPlayerCell;
			
			float distance = Vector3.Distance(transform.localPosition, closestPlayer.transform.position);
			if (distance < Constants.GhostClydeNumberOfCellsDistance)
			{
				targetCell = new Vector3Int(Mathf.RoundToInt(ScatterBasePosition.position.x), Mathf.RoundToInt(ScatterBasePosition.position.y), 0);
			}

			return targetCell;
		}
	}
}
