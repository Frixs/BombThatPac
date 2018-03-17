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
        /// Input HOTKEY to MOVE UP.
        /// </summary>
        [SerializeField] protected KeyCode InputMoveUp = KeyCode.W;

        /// <summary>
        /// Input HOTKEY to MOVE LEFT.
        /// </summary>
        [SerializeField] protected KeyCode InputMoveLeft = KeyCode.A;

        /// <summary>
        /// Input HOTKEY to MOVE DOWN.
        /// </summary>
        [SerializeField] protected KeyCode InputMoveDown = KeyCode.S;

        /// <summary>
        /// Input HOTKEY to MOVE RIGHT.
        /// </summary>
        [SerializeField] protected KeyCode InputMoveRight = KeyCode.D;

        /// <summary>
        /// Input HOTKEY to ACTION.
        /// </summary>
        [SerializeField] protected KeyCode InputAction = KeyCode.Space;

        /// <summary>
        /// Input HOTKEY to SPECIAL ACTION.
        /// </summary>
        [SerializeField] protected KeyCode InputSpecialAction = KeyCode.R;

        /// <summary>
        /// Input HOTKEY to COLLECT ITEM.
        /// </summary>
        [SerializeField] protected KeyCode InputCollectItem = KeyCode.F;

        /// <summary>
        /// Reference to bomb PREFAB.
        /// </summary>
        [SerializeField] private GameObject _bombPrefab;
        
        /// <summary>
        /// Counter of bomb deploys.
        /// </summary>
        public int BombDeployCounter { get; set; } = 0;

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

            if (Input.GetKey(InputMoveUp))
            {
                Direction += Vector2.up;
            }
            else if (Input.GetKey(InputMoveLeft))
            {
                Direction += Vector2.left;
            }
            else if (Input.GetKey(InputMoveDown))
            {
                Direction += Vector2.down;
            }
            else if (Input.GetKey(InputMoveRight))
            {
                Direction += Vector2.right;
            }

            if (Input.GetKeyDown(InputAction))
            {
                DeployTheBomb();
            }
            
            if (Input.GetKeyDown(InputSpecialAction))
            {
                DeployTheBomb();
            }
            
            if (Input.GetKeyDown(InputCollectItem))
            {
                DeployTheBomb();
            }
        }

        /// <summary>
        /// Deploy the bomb on the correct position.
        /// </summary>
        protected void DeployTheBomb()
        {
            if (BombDeployCounter >= BombMaxAllowedDeploys)
                return;

            BombDeployCounter++;
            
            Vector3Int cell = FindObjectOfType<MapManager>().TilemapGameplay.WorldToCell(transform.position);
            Vector3 cellCenterPos = FindObjectOfType<MapManager>().TilemapGameplay.GetCellCenterWorld(cell);

            (Instantiate(_bombPrefab, cellCenterPos, Quaternion.identity) as GameObject).GetComponent<Bomb>().Owner = this;
            
            Debug.unityLogger.LogFormat(LogType.Log, "[{0}] Bomb deployed!", Name);
        }
    }
}