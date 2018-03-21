using Characters;
using Managers;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Items
{
	public class Bomb : MonoBehaviour
	{
		/// <summary>
		/// Reference to bomb owner.
		/// </summary>
		[HideInInspector] public Player Caster;

		/// <summary>
		/// Bomb's countdown.
		/// </summary>
		[HideInInspector] public float Countdown;
		
		/// <summary>
		/// Reference to explosion PREFAB.
		/// </summary>
		public GameObject ExplosionPrefab;

		/// <summary>
		/// Range when the bomb become collidable after placing it on the ground.
		/// </summary>
		private float _rangeToBecomeCollidable;

		// Use this for initialization
		void Start()
		{
			Countdown = Caster.BombCountdown;
			_rangeToBecomeCollidable = MapManager.Instance.TilemapCellSize / 2f;
			
			// Ignore collision when spawning under your feet.
			Physics2D.IgnoreCollision(Caster.GetComponent<CircleCollider2D>(), GetComponent<CircleCollider2D>());
		}
	
		// Update is called once per frame
		void Update()
		{
			Countdown -= Time.deltaTime;

			if (Countdown <= 0.0f)
			{
				Explode(transform.position);
				Debug.unityLogger.LogFormat(LogType.Log, "[{0} ({1})] Bomb exploded!", Caster.Identifier, Caster.Name);
				
				Caster.BombDeployCounter--;
				
				Destroy(gameObject);
			}
		}

		// FixedUpdate is called every fixed framerate frame
		void FixedUpdate()
		{
			// Restore the collision when player will be in sufficient distance from the bomb.
			if (Vector2.Distance(Caster.transform.position, transform.position) > _rangeToBecomeCollidable)
			{
				Physics2D.IgnoreCollision(Caster.GetComponent<CircleCollider2D>(), GetComponent<CircleCollider2D>(), false);
			}
		}
		
		/// <summary>
		/// Bomb explosion.
		/// </summary>
		/// <param name="worldPos">Position of epicenter.</param>
		public void Explode(Vector2 worldPos)
		{
			Vector3Int originCell = MapManager.Instance.TilemapGameplay.WorldToCell(worldPos);
			
			MapManager.Instance.ExplodeInCell(originCell, Caster);
			
			for (int i = 0; i < Caster.BombExplosionDirection.GetLength(0); i++)
			{
				ExplodeInDirection(originCell, new Vector3Int(Caster.BombExplosionDirection[i, 0], Caster.BombExplosionDirection[i, 1], Caster.BombExplosionDirection[i, 2]));
			}
		}

		/// <summary>
		/// Explosion in certain direction.
		/// </summary>
		/// <param name="origin">Origion of the explosion.</param>
		/// <param name="direction">Direction of explosion - Vector3Int filled only with -1, 0, 1 values. Each direction can has only1 value rest 0. We are on grid only.</param>
		private void ExplodeInDirection(Vector3Int origin, Vector3Int direction)
		{
			for (int i = 1; i <= Caster.BombExplosionDistance; i++)
			{
				if (!MapManager.Instance.ExplodeInCell(origin + direction * i, Caster))
					break;
			}
		}
	}
}
