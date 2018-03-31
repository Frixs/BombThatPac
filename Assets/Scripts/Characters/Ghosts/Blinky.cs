using System;
using Managers;
using UnityEngine;

namespace Characters
{
	[Serializable]
	public class Blinky : Ghost
	{
		// Use this for initialization
		protected override float GhostReleaseTimer { get; set; } = Constants.GhostBlinkyReleaseTimer;
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
			return MapManager.Instance.TilemapGameplay.WorldToCell(GetClosestPlayer().transform.position);
		}
	}
}
