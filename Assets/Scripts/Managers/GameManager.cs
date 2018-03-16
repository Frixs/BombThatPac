using UnityEngine;
using UnityEngine.Tilemaps;

namespace Managers
{
	public class GameManager : MonoBehaviour
	{
		/// <summary>
		/// Reference to the gameplay tilemap.
		/// </summary>
		[SerializeField] private Tilemap _tilemapGameplay;
		public Tilemap GetTilemapGameplay()
		{
			return _tilemapGameplay;
		}
	
		// Use this for initialization
		void Start () {
		}
	
		// Update is called once per frame
		void Update () {
		
		}
	}
}
