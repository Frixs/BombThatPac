using Items;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Managers
{
    public class MapManager : MonoBehaviour
    {
        /// <summary>
        /// Static instance of MapManager which allows it to be accessed by any other script.
        /// </summary>
        public static MapManager Instance = null;
        
        /// <summary>
        /// Reference of wall tile.
        /// </summary>
        [SerializeField] private TileBase _wallTile = null;

        /// <summary>
        /// Reference of detructable tile.
        /// </summary>
        [SerializeField] private Tile _destructibleTile = null;
        
        /// <summary>
        /// Reference to the gameplay tilemap.
        /// </summary>
        [SerializeField] private Tilemap _tilemapGameplay;
        
        public Tilemap TilemapGameplay
        {
            get { return _tilemapGameplay; }
        }

        // Awake is always called before any Start functions
        void Awake()
        {
            // Check if instance already exists.
            if (Instance == null)
            {
                // If not, set instance to this.
                Instance = this;
            }
            // If instance already exists and it's not this.
            else if (Instance != this)
            {
                // Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a MapManager.
                Destroy(gameObject);
            }
        }
        
        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        /// <summary>
        /// Make explosion in the current cell (by world position pointing to the cell.)
        /// </summary>
        /// <param name="cell">World position of the cell.</param>
        /// <returns></returns>
        public bool ExplodeInCell(Vector3Int cell)
        {
            TileBase tile = _tilemapGameplay.GetTile<TileBase>(cell);
            
            // End an explosion if explosion wants to hit obstacle.
            if (tile == _wallTile)
                return false;
            
            if (tile == _destructibleTile)
            {
                // Remove the tile.
                _tilemapGameplay.SetTile(cell, null);
            }
            else if (true)
            {
                //TODO Physics2D.OverlapCircle(new Vector2(cell.x, cell.y), TilemapGameplay.cellSize.sqrMagnitude / 2.0f);
            }

            // Create an explosion.
            Vector3 pos = _tilemapGameplay.GetCellCenterWorld(cell);
            GameObject explosion = (GameObject) Instantiate(FindObjectOfType<Bomb>().ExplosionPrefab, pos, Quaternion.identity);
        
            // Destroy the explosion after animation.
            Destroy(explosion, FindObjectOfType<Bomb>().ExplosionPrefab.GetComponent<Animator>().runtimeAnimatorController.animationClips.Length);
        
            return true;
        }
    }
}