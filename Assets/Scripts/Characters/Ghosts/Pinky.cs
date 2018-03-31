using Managers;
using UnityEngine;

namespace Characters
{
	public class Pinky : Ghost
	{
		// Use this for initialization
		protected override float GhostReleaseTimer { get; set; } = Constants.GhostPinkyReleaseTimer;
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
			// X cells ahead of closest player.

			Player closestPlayer = GetClosestPlayer();
			Vector3Int closestPlayerCell = MapManager.Instance.TilemapGameplay.WorldToCell(closestPlayer.transform.position);
			Vector2 playerOrientation = closestPlayer.GetOrientation();

			return closestPlayerCell +
			       new Vector3Int(Mathf.RoundToInt(playerOrientation.x), Mathf.RoundToInt(playerOrientation.y), 0) * Constants.GhostPinkyNumberOfCellsAheadToTarget;
		}
	}
}
