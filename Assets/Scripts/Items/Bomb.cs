using System.Security.Cryptography.X509Certificates;
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
		}
	
		// Update is called once per frame
		void Update()
		{
			_countdown -= Time.deltaTime;

			if (_countdown <= 0.0f)
			{
				Debug.unityLogger.LogFormat(LogType.Log, "[{0}] Bomb exploded!", Owner.Name);
				Explode(transform.position);
				Destroy(gameObject);
			}
		}
		
		/// <summary>
		/// Bomb explosion.
		/// </summary>
		/// <param name="worldPos">Position of epicenter.</param>
		public void Explode(Vector2 worldPos)
		{
			Vector3Int originCell = FindObjectOfType<MapManager>().TilemapGameplay.WorldToCell(worldPos);
			bool[] isCollisionInDirection = new bool[4] {false, false, false, false};

			FindObjectOfType<MapManager>().ExplodeInCell(originCell);
			for (int i = 1; i <= Owner.BombExplosionDistance; i++)
			{
				if (!isCollisionInDirection[0] && !FindObjectOfType<MapManager>().ExplodeInCell(originCell + new Vector3Int(i, 0, 0)))
					isCollisionInDirection[0] = true;
				
				if (!isCollisionInDirection[1] && !FindObjectOfType<MapManager>().ExplodeInCell(originCell + new Vector3Int(0, i, 0)))
					isCollisionInDirection[1] = true;
				
				if (!isCollisionInDirection[2] && !FindObjectOfType<MapManager>().ExplodeInCell(originCell + new Vector3Int(-i, 0, 0)))
					isCollisionInDirection[2] = true;
				
				if (!isCollisionInDirection[3] && !FindObjectOfType<MapManager>().ExplodeInCell(originCell + new Vector3Int(0, -i, 0)))
					isCollisionInDirection[3] = true;
			}
		}
	}
}
