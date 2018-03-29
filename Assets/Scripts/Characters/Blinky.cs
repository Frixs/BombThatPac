using Managers;
using UnityEngine;

namespace Characters
{
	public class Blinky : Ghost
	{
		// Use this for initialization
		protected override float GhostReleaseTimer { get; set; } = Constants.GhostBlinkyReleaseTimer;
		protected override Transform SpawnPosition { get; set; }
		protected override Transform ScatterBasePosition { get; set; }
		protected override Transform StartTargetPosition { get; set; }

		// Awake is always called before any Start functions
		protected void Awake()
		{
			SpawnPosition = GameObject.Find("GhostBlinkySpawnPoint").transform;
			ScatterBasePosition = GameObject.Find("GhostBlinkyScatterBase").transform;
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
			return MapManager.Instance.TilemapGameplay.WorldToCell(GetClosestPlayer().transform.position);
		}
	}
}
