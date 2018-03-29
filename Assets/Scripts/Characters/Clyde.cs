using Managers;
using UnityEngine;

namespace Characters
{
	public class Clyde : Ghost
	{
		// Use this for initialization
		protected override float GhostReleaseTimer { get; set; } = Constants.GhostClydeReleaseTimer;
		protected override Transform SpawnPosition { get; set; }
		protected override Transform ScatterBasePosition { get; set; }
		protected override Transform StartTargetPosition { get; set; }

		// Awake is always called before any Start functions
		protected void Awake()
		{
			SpawnPosition = GameObject.Find("GhostClydeSpawnPoint").transform;
			ScatterBasePosition = GameObject.Find("GhostClydeScatterBase").transform;
			StartTargetPosition = GameObject.Find("GhostStartTargetPositionPoint").transform;
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
