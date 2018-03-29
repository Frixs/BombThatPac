using UnityEngine;

namespace Characters
{
	public abstract class ArtificialIntelligence : Character
	{
		protected override void Start()
		{
			base.Start();
		}
	
		// Update is called once per frame
		protected override void Update()
		{
			base.Update();
		}
		
		// Fixed update
		protected override void FixedUpdate()
		{
			base.FixedUpdate();
		}
		
		public override Vector2 GetOrientation()
		{
			Vector2 orientation = Vector2.zero;

			if (Direction.y > 0)
				orientation = Vector2.up;
			else if (Direction.x < 0)
				orientation = Vector2.left;
			else if (Direction.y < 0)
				orientation = Vector2.down;
			else if (Direction.x > 0)
				orientation = Vector2.right;
            
			return orientation;
		}
	}
}
