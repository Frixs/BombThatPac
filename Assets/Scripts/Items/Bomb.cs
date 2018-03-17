using Characters;
using Managers;
using UnityEngine;

namespace Items
{
	public class Bomb : MonoBehaviour
	{
		/// <summary>
		/// Reference to bomb owner.
		/// </summary>
		public Player Owner { private get; set; }

		/// <summary>
		/// Bomb's countdown.
		/// </summary>
		private float _countdown;
		
		/// <summary>
		/// Reference to explosion PREFAB.
		/// </summary>
		[SerializeField] private GameObject _explosionPrefab;

		public GameObject ExplosionPrefab
		{
			get { return _explosionPrefab; }
		}

		// Use this for initialization
		void Start()
		{
			_countdown = Owner.BombCountdown;
			
			// Ignore collision when spawning under your feet.
			Physics2D.IgnoreCollision(Owner.GetComponent<CircleCollider2D>(), GetComponent<CircleCollider2D>());
		}
	
		// Update is called once per frame
		void Update()
		{
			_countdown -= Time.deltaTime;

			if (_countdown <= 0.0f)
			{
				Explode(transform.position);
				Debug.unityLogger.LogFormat(LogType.Log, "[{0}] Bomb exploded!", Owner.Name);
				
				Owner.BombDeployCounter--;
				
				Destroy(gameObject);
			}
		}

		// FixedUpdate is called every fixed framerate frame
		void FixedUpdate()
		{
			// Restore the collision when player will be in sufficient distance from the bomb.
			if (Vector2.Distance(Owner.transform.position, transform.position) > FindObjectOfType<MapManager>().TilemapGameplay.cellSize.sqrMagnitude / 3.5f)
			{
				Physics2D.IgnoreCollision(Owner.GetComponent<CircleCollider2D>(), GetComponent<CircleCollider2D>(), false);
			}
		}
		
		/// <summary>
		/// Bomb explosion.
		/// </summary>
		/// <param name="worldPos">Position of epicenter.</param>
		public void Explode(Vector2 worldPos)
		{
			Vector3Int originCell = FindObjectOfType<MapManager>().TilemapGameplay.WorldToCell(worldPos);
			
			FindObjectOfType<MapManager>().ExplodeInCell(originCell);
			
			for (int i = 0; i < Owner.BombExplosionDirection.GetLength(0); i++)
			{
				ExplodeInDirection(originCell, new Vector3Int(Owner.BombExplosionDirection[i, 0], Owner.BombExplosionDirection[i, 1], Owner.BombExplosionDirection[i, 2]));
			}
		}

		/// <summary>
		/// Explosion in certain direction.
		/// </summary>
		/// <param name="origin">Origion of the explosion.</param>
		/// <param name="direction">Direction of explosion - Vector3Int filled only with -1, 0, 1 values. Each direction can has only1 value rest 0. We are on grid only.</param>
		private void ExplodeInDirection(Vector3Int origin, Vector3Int direction)
		{
			for (int i = 1; i <= Owner.BombExplosionDistance; i++)
			{
				if (!FindObjectOfType<MapManager>().ExplodeInCell(origin + direction * i))
					break;
			}
		}
	}
}
