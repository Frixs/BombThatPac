using Items;
using Managers;
using UnityEngine;

namespace Characters
{
    /// <summary>
    /// Player controlled character.
    /// </summary>
    public abstract class Player : Character
    {
        /// <summary>
        /// Default number of the bombs that can be placed on the same time.
        /// </summary>
        public abstract int BombStackCount { get; set; }

        /// <summary>
        /// Bomb countdown.
        /// </summary>
        public abstract float BombCountdown { get; set; }

        /// <summary>
        /// Bomb explosion distance in tiles.
        /// </summary>
        public abstract int BombExplosionDistance { get; set; }
        
        /// <summary>
        /// Maximal number of deployed bombs at the same time.
        /// </summary>
        public abstract int BombMaxAllowedDeploys { get; set; }
        
        /// <summary>
        /// Explosion direction represented as array filled with X, Y coords representing the direction.
        /// </summary>
        public abstract int[,] BombExplosionDirection { get; set; }

        /// <summary>
        /// Lets say which section of player controls the character should use.
        /// </summary>
        public string InputPlayerSection;

        /// <summary>
        /// Reference to bomb PREFAB.
        /// </summary>
        [SerializeField] private GameObject _bombPrefab;
        
        /// <summary>
        /// Counter of bomb deploys.
        /// </summary>
        [HideInInspector] public int BombDeployCounter = 0;

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        protected override void Update()
        {
            GetInput();

            base.Update();
        }

        /// <summary>
        /// Listen's to the players input.
        /// </summary>
        protected void GetInput()
        {
            Direction = Vector2.zero;

            if (!HasEnabledActions)
                return;

            if (Input.GetKey(InputManager.Instance.GetButtonKeyCode(InputPlayerSection, "MoveUp")))
            {
                Direction += Vector2.up;
            }
            else if (Input.GetKey(InputManager.Instance.GetButtonKeyCode(InputPlayerSection, "MoveLeft")))
            {
                Direction += Vector2.left;
            }
            else if (Input.GetKey(InputManager.Instance.GetButtonKeyCode(InputPlayerSection, "MoveDown")))
            {
                Direction += Vector2.down;
            }
            else if (Input.GetKey(InputManager.Instance.GetButtonKeyCode(InputPlayerSection, "MoveRight")))
            {
                Direction += Vector2.right;
            }

            if (Input.GetKeyDown(InputManager.Instance.GetButtonKeyCode(InputPlayerSection, "Action")))
            {
                PlaceBomb();
            }
            
            if (Input.GetKeyDown(InputManager.Instance.GetButtonKeyCode(InputPlayerSection, "SpecialAction")))
            {
                PlaceBomb();
            }
            
            if (Input.GetKeyDown(InputManager.Instance.GetButtonKeyCode(InputPlayerSection, "CollectItem")))
            {
                PlaceBomb();
            }
        }

        /// <summary>
        /// Place the bomb on the correct position.
        /// </summary>
        protected void PlaceBomb()
        {
            if (BombDeployCounter >= BombMaxAllowedDeploys)
                return;

            BombDeployCounter++;
            
            Vector3Int cell = MapManager.Instance.TilemapGameplay.WorldToCell(transform.position);
            Vector3 cellCenterPos = MapManager.Instance.TilemapGameplay.GetCellCenterWorld(cell);

            (Instantiate(_bombPrefab, cellCenterPos, Quaternion.identity) as GameObject).GetComponent<Bomb>().Owner = this;
            
            Debug.unityLogger.LogFormat(LogType.Log, "[{0} ({1})] Bomb deployed!", PlayerNumber, Name);
        }
    }
}