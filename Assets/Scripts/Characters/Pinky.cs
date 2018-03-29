using Managers;
using UnityEngine;

namespace Characters
{
	public class Pinky : Ghost
	{
		// Use this for initialization
		protected override float GhostReleaseTimer { get; set; } = Constants.GhostPinkyReleaseTimer;
		protected override Transform SpawnPosition { get; set; }
		protected override Transform ScatterBasePosition { get; set; }
		protected override Transform StartTargetPosition { get; set; }

		// Awake is always called before any Start functions
		protected void Awake()
		{
			SpawnPosition = GameObject.Find("GhostPinkySpawnPoint").transform;
			ScatterBasePosition = GameObject.Find("GhostPinkyScatterBase").transform;
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
			// X cells ahead of closest player.

			Player closestPlayer = GetClosestPlayer();
			Vector3Int closestPlayerCell = MapManager.Instance.TilemapGameplay.WorldToCell(closestPlayer.transform.position);
			Vector2 playerOrientation = closestPlayer.GetOrientation();

			return closestPlayerCell +
			       new Vector3Int(Mathf.RoundToInt(playerOrientation.x), Mathf.RoundToInt(playerOrientation.y), 0) * Constants.GhostPinkyNumberOfCellsAheadToTarget;
		}
	}
}
