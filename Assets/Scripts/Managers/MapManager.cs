using Characters;
using Items;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Managers
{
    /// <summary>
    /// This class works with map tiles and  launches events like explosions in the map.
    /// </summary>
    public class MapManager : MonoBehaviour
    {
        /// <summary>
        /// Static instance of MapManager which allows it to be accessed by any other script.
        /// </summary>
        public static MapManager Instance = null;

        /// <summary>
        /// List of all possible player spawn positions.
        /// </summary>
        [HideInInspector] public Transform[] PlayerSpawnPoints;

        /// <summary>
        /// Reference of wall tile.
        /// </summary>
        public TileBase WallTile => _wallTile;
        [SerializeField] private TileBase _wallTile;

        /// <summary>
        /// Reference of detructable tile.
        /// </summary>
        public Tile DestructibleTile => _destructibleTile;
        [SerializeField] private Tile _destructibleTile;
        
        /// <summary>
        /// Reference to door game object.
        /// </summary>
        public GameObject DoorObject => _doorObject;
        [SerializeField] private GameObject _doorObject;

        /// <summary>
        /// Reference to the gameplay tilemap.
        /// </summary>
        public Tilemap TilemapGameplay;

        /// <summary>
        /// Constant of cell size.
        /// </summary>
        public float TilemapCellSize { get; private set; }

        /// <summary>
        /// Constant of cell half size.
        /// </summary>
        public float TilemapCellHalfSize { get; private set; }

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

            // Initialize important constants.
            TilemapCellSize = TilemapGameplay.cellSize.x;
            TilemapCellHalfSize = TilemapCellSize / 2f;
        }

        // Use this for initialization
        void Start()
        {
            GameObject playerSpawnPointHandler = GameObject.Find("PlayerSpawnPoints");
            
            PlayerSpawnPoints = new Transform[playerSpawnPointHandler.transform.childCount];
            
            for (int i = 0; i < playerSpawnPointHandler.transform.childCount; i++)
            {
                PlayerSpawnPoints[i] = playerSpawnPointHandler.transform.GetChild(i);
            }
        }

        // Update is called once per frame
        void Update()
        {
        }

        /// <summary>
        /// Make explosion in the current cell (by world position pointing to the cell.)
        /// </summary>
        /// <param name="cell">World position of the cell.</param>
        /// <param name="caster">Reference to caster of an effect which caused the explosion.</param>
        /// <returns></returns>
        public bool ExplodeInCell(Vector3Int cell, Character caster)
        {
            TileBase tile = TilemapGameplay.GetTile<TileBase>(cell);

            // Try to find obstacle in the current cell.
            Collider2D[] obstacles = Physics2D.OverlapCircleAll(
                new Vector2(
                    cell.x + TilemapCellHalfSize,
                    cell.y + TilemapCellHalfSize
                ),
                TilemapCellHalfSize,
                1 << LayerMask.NameToLayer(Constants.UserLayerNameObstacle)
            );
            
            // End an explosion if explosion wants to hit obstacle.
            if (tile == WallTile || obstacles.Length > 0)
                return false;

            if (tile == DestructibleTile)
            {
                // Remove the tile.
                TilemapGameplay.SetTile(cell, null);
                return false;
            }
            else
            {
                // Find all characters affected by the explosion.
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(
                    new Vector2(
                        cell.x + TilemapCellHalfSize,
                        cell.y + TilemapCellHalfSize
                    ),
                    TilemapCellHalfSize,
                    1 << LayerMask.NameToLayer(Constants.UserLayerNameTriggerObject)
                );

                // Go through all characters affected by the explosion.
                for (var i = 0; i < hitColliders.Length; i++)
                {
                    Component component = null;

                    if ((component = hitColliders[i].GetComponent<Player>()) != null)
                    {
                        ((Player) component).Kill(caster);
                    }

                    if ((component = hitColliders[i].GetComponent<Bomb>()) != null)
                    {
                        ((Bomb) component).Countdown = Constants.BombChainedCountdown;
                    }
                }
            }

            // Create an explosion.
            Vector3 pos = TilemapGameplay.GetCellCenterWorld(cell);
            GameObject explosion = (GameObject) Instantiate(FindObjectOfType<Bomb>().ExplosionPrefab, pos, Quaternion.identity);

            // Destroy the explosion after animation.
            Destroy(explosion, FindObjectOfType<Bomb>().ExplosionPrefab.GetComponent<Animator>().runtimeAnimatorController.animationClips.Length);

            return true;
        }
    }
}