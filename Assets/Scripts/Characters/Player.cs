using Items;
using Managers;
using UnityEngine;

namespace Characters
{
    /// <inheritdoc />
    /// <summary>
    /// Player controlled character.
    /// </summary>
    public abstract class Player : Character
    {
        /// <summary>
        /// The Player's movement speed.
        /// </summary>
        protected override float Speed { get; set; } = Constants.PlayerDefaultSpeed;
        
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
        [HideInInspector] public string InputPlayerSection;

        /// <summary>
        /// Reference to bomb PREFAB.
        /// </summary>
        [SerializeField] private GameObject _bombPrefab;
        
        /// <summary>
        /// Counter of bomb deploys.
        /// </summary>
        [HideInInspector] public int BombDeployCounter = 0;

        /// <summary>
        /// Counts player collected fragments.
        /// </summary>
        [HideInInspector] public int FragmentCounter = 0;

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

        public override void Move()
        {
            // Makes sure that the player moves.
            MyRigidBody.velocity = Direction.normalized * Speed * (HasEnabledActions ? 1 : 0);
        }

        public override Vector2 GetOrientation()
        {
            Vector2 orientation = Vector2.zero;

            if (PreviousDirection.y > 0)
                orientation = Vector2.up;
            else if (PreviousDirection.x < 0)
                orientation = Vector2.left;
            else if (PreviousDirection.y < 0)
                orientation = Vector2.down;
            else if (PreviousDirection.x > 0)
                orientation = Vector2.right;
            
            return orientation;
        }
        
        /// <summary>
        /// Listen's to the players input.
        /// </summary>
        protected void GetInput()
        {
            if (Direction != Vector2.zero && Direction != PreviousDirection)
                PreviousDirection = Direction;
            
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
            }
            
            if (Input.GetKeyDown(InputManager.Instance.GetButtonKeyCode(InputPlayerSection, "CollectItem")))
            {
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

            (Instantiate(_bombPrefab, cellCenterPos, Quaternion.identity) as GameObject).GetComponent<Bomb>().Caster = this;
            
            Debug.unityLogger.LogFormat(LogType.Log, "[{0} ({1})] Bomb planted!", Identifier, Name);
        }
        
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="other"></param>
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Fragment"))
            {
                FragmentCounter++;
                Destroy(other.gameObject);
            }
        }

        /// <summary>
        /// Kill the character.
        /// </summary>
        /// <param name="attacker">Reference to attacker character.</param>
        public override void Kill(Character attacker)
        {
            if (!IsKillable() || !attacker)
                return;

            IsDeath = true;
            
            if (IsRespawnable)
                SpawnManager.Instance.RespawnCharacterInit(gameObject, RespawnDeathDelay, MapManager.Instance.PlayerSpawnPoints);

            gameObject.SetActive(false);
            Debug.unityLogger.LogFormat(LogType.Log, "[{0} ({1})] character has been killed by character: [{2} ({3})]!", Identifier, Name, attacker.Identifier, attacker.Name);
        }

        /// <summary>
        /// Immediately kills the character without any reason.
        /// </summary>
        /// <param name="respawn">TRUE for respawn after death. FALSE for no respawn anymore.</param>
        public override void ForceKill(bool respawn)
        {
            if (!respawn)
                IsRespawnable = false;
            
            IsDeath = true;
            
            if (IsRespawnable)
                SpawnManager.Instance.RespawnCharacterInit(gameObject, RespawnDeathDelay, MapManager.Instance.PlayerSpawnPoints);
            
            gameObject.SetActive(false);
            Debug.unityLogger.LogFormat(LogType.Log, "[{0} ({1})] Character has been force killed!", Identifier, Name);
        }
    }
}