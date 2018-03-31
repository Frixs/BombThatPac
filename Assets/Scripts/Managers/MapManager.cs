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

        /// <summary>
        /// Reference of wall tile.
        /// </summary>
        public TileBase WallTile => _wallTile;

        [Header("Enviroment")] [SerializeField]
        private TileBase _wallTile;

        /// <summary>
        /// Reference of detructable tile.
        /// </summary>
        public Tile DestructibleTile => _destructibleTile;

        [SerializeField] private Tile _destructibleTile;

        /// <summary>
        /// Reference to door game object.
        /// </summary>
        public GameObject PlayerSpawnPointPrefab => _playerSpawnPointPrefab;

        [Header("Player Points")] [SerializeField]
        private GameObject _playerSpawnPointPrefab;
        
        /// <summary>
        /// List of all possible player spawn positions.
        /// </summary>
        [HideInInspector] public Transform[] PlayerSpawnPoints;
        
        /// <summary>
        /// Reference to door game object.
        /// </summary>
        public GameObject GhostDoorPrefab => _ghostDoorPrefab;

        [Header("Ghost Points")] [SerializeField]
        private GameObject _ghostDoorPrefab;
        
        /// <summary>
        /// Reference to door game object trigger event handler.
        /// </summary>
        public GameObject GhostDoorTriggerPrefab => _ghostDoorTriggerPrefab;

        [SerializeField]
        private GameObject _ghostDoorTriggerPrefab;
        
        /// <summary>
        /// Reference to ghost start target position. This is the first position whre they should go.
        /// </summary>
        public GameObject GhostStartTargetPositionPointPrefab => _ghostStartTargetPositionPointPrefab;

        [SerializeField]
        private GameObject _ghostStartTargetPositionPointPrefab;

        /// <summary>
        /// Reference to ghost spawn positions. Each ghost has only 1 own spawn position. Depends on the order!!!
        /// </summary>
        public GameObject[] GhostSpawnPointPrefabs => _ghostSpawnPointPrefabs;

        [SerializeField] private GameObject[] _ghostSpawnPointPrefabs;

        /// <summary>
        /// Reference to ghost scatter home positions. Each ghost has only 1 own scatter home position. Depends on the order!!!
        /// </summary>
        public GameObject[] GhostScatterHomePrefabs => _ghostScatterHomePrefabs;

        [SerializeField] private GameObject[] _ghostScatterHomePrefabs;

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
    }
}