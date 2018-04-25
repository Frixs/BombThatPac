using Items;
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
        public RuleTile DestructableObstacleTile => _destructableObstacleTile;

        [SerializeField] private RuleTile _destructableObstacleTile;

        /// <summary>
        /// List of all possible player spawn positions.
        /// </summary>
        [HideInInspector] public Transform[] PlayerSpawnPoints;
        
        /// <summary>
        /// List of all possible item spawn positions.
        /// </summary>
        [HideInInspector] public Transform[] ItemSpawnPoints;
        
        /// <summary>
        /// List of all possible finish spawn positions.
        /// </summary>
        [HideInInspector] public Transform[] FinishSpawnPoints;
        
        /// <summary>
        /// Total fragment count in the maze.
        /// </summary>
        [HideInInspector] public int TotalFragmentCount;

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
            // Initialize important constants.
            TilemapCellSize = TilemapGameplay.cellSize.x;
            TilemapCellHalfSize = TilemapCellSize / 2f;

            Setup();
        }

        // Update is called once per frame
        void Update()
        {
        }

        /// <summary>
        /// Initialization of a map.
        /// </summary>
        public void Setup()
        {
            FindPlayerSpawnPoints();
            FindItemSpawnPoints();
            FindFinishSpawnPoints();
            FindAndSetFragmentCount();
        }

        /// <summary>
        /// Find and set all player spawn points.
        /// </summary>
        private void FindPlayerSpawnPoints()
        {
            GameObject playerSpawnPointHandler = GameObject.Find("PlayerSpawnPoints");
            PlayerSpawnPoints = new Transform[playerSpawnPointHandler.transform.childCount];
            for (int i = 0; i < playerSpawnPointHandler.transform.childCount; i++)
                PlayerSpawnPoints[i] = playerSpawnPointHandler.transform.GetChild(i);
        }
        
        /// <summary>
        /// Find and set all item spawn points.
        /// </summary>
        private void FindItemSpawnPoints()
        {
            GameObject itemSpawnPointHandler = GameObject.Find("ItemSpawnPoints");
            ItemSpawnPoints = new Transform[itemSpawnPointHandler.transform.childCount];
            for (int i = 0; i < itemSpawnPointHandler.transform.childCount; i++)
                ItemSpawnPoints[i] = itemSpawnPointHandler.transform.GetChild(i);
        }
        
        /// <summary>
        /// Find and set all finish spawn points.
        /// </summary>
        private void FindFinishSpawnPoints()
        {
            GameObject finishSpawnPointHandler = GameObject.Find("FinishSpawnPoints");
            FinishSpawnPoints = new Transform[finishSpawnPointHandler.transform.childCount];
            for (int i = 0; i < finishSpawnPointHandler.transform.childCount; i++)
                FinishSpawnPoints[i] = finishSpawnPointHandler.transform.GetChild(i);
        }

        /// <summary>
        /// Find and set total fragment count on a map.
        /// </summary>
        private void FindAndSetFragmentCount()
        {
            // Update only once. This set up the number of fragments in the maze.
            TotalFragmentCount = GameObject.Find("Fragments").GetComponentsInChildren<ItemFragment>().Length;
        }
    }
}