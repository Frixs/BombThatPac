using Managers;
using UnityEngine;

namespace Characters
{
    /// <summary>
    /// Player controlled character.
    /// </summary>
    public class Player : Character
    {
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
        /// Default number of the bombs that can be placed on the same time.
        /// </summary>
        protected int BombStackCount = Constants.PlayerDefaultBombStackCount;

        /// <summary>
        /// Bomb countdown.
        /// </summary>
        protected float BombCountdown = Constants.BombDefaultCountdown;

        /// <summary>
        /// Reference to bomb PREFAB.
        /// </summary>
        [SerializeField] private GameObject _bombPrefab;

        // Use this for initialization
        protected virtual void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        protected virtual void Update()
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
        /// TODO
        /// </summary>
        protected void DeployTheBomb()
        {
            Debug.unityLogger.LogFormat(LogType.Log, "[{0}] Bomb deployed!", Name);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(transform.position);
            Vector3Int cell = GetComponent<GameManager>().GetTilemapGameplay().WorldToCell(worldPos);
            Vector3 cellCenterPos = GetComponent<GameManager>().GetTilemapGameplay().GetCellCenterWorld(cell);

            Instantiate(_bombPrefab, cellCenterPos, Quaternion.identity);
        }
    }
}