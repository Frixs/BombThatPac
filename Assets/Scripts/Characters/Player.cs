using System.Collections.Generic;
using System.Linq;
using Items;
using Items.SpecialItems;
using Managers;
using StatusEffects;
using StatusEffects.Scriptable;
using UnityEngine;
using UnityEngine.UI;

namespace Characters
{
    /// <inheritdoc />
    /// <summary>
    /// Player controlled character.
    /// </summary>
    public abstract class Player : Character
    {
        /// <summary>
        /// Reference to the player. This is just game object (Player) which player controls.
        /// </summary>
        [HideInInspector] public PlayerManager PlayerManagerReference;
        
        /// <summary>
        /// The Player's movement speed.
        /// </summary>
        public override float MoveSpeed { get; set; } = Constants.PlayerDefaultMoveSpeed;
        
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
        /// All items represented as scriptable status effect the player currently has.
        /// </summary>
        [HideInInspector] public List<ScriptableStatusEffect> SpecialItemList = new List<ScriptableStatusEffect>();
        
        /// <summary>
        /// Reference to bomb PREFAB.
        /// </summary>
        [Header("Prefab References")] [SerializeField] private GameObject _bombPrefab;
        
        /// <summary>
        /// Reference to fragment PREFAB.
        /// </summary>
        [SerializeField] private GameObject _fragmentPrefab;

        /// <summary>
        /// Scriptable asset of invulnerability status effect buff after respawn player.
        /// </summary>
        public ScriptableStatusEffect RespawnInvulStatusEffect => _respawnInvulStatusEffect;

        [Header("Status Effects")] [SerializeField] private ScriptableStatusEffect _respawnInvulStatusEffect;
        
        /// <summary>
        /// Lets say which section of player controls the character should use.
        /// </summary>
        [HideInInspector] public string InputPlayerSection;
        
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
        
        void OnTriggerEnter2D(Collider2D other)
        {
            // FRAGMENT.
            if (other.gameObject.CompareTag("Fragment"))
            {
                FragmentCounter += other.GetComponent<Fragment>().Quantity;
                Destroy(other.gameObject);
            }
            // CHERRY.
            else if (other.gameObject.CompareTag("Cherry"))
            {
                for (int i = 0; i < GameManager.Instance.Ghosts.Length; i++)
                {
                    GameManager.Instance.Ghosts[i].StartFrightenedMode();
                }
                
                Destroy(other.gameObject);
            }
            // SPECIAL ITEM.
            else if (other.gameObject.CompareTag("SpecialItem"))
            {
                PlayerManagerReference.PlayerPanelReference.PlayerInventory.AddItem(this, other.GetComponent<SpecialItem>());
                Destroy(other.gameObject);
            }
        }

        public override void Move()
        {
            // Makes sure that the player moves.
            MyRigidBody.velocity = Direction.normalized * MoveSpeed * (HasEnabledActions ? 1 : 0);
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
                UseSpecialItem();
            }
            
            if (Input.GetKeyDown(InputManager.Instance.GetButtonKeyCode(InputPlayerSection, "CollectItem")))
            {
                // Nothing.
            }
        }

        /// <summary>
        /// Place the bomb on the correct position.
        /// </summary>
        protected void PlaceBomb()
        {
            if (BombDeployCounter >= BombMaxAllowedDeploys)
                return;

            Vector3Int cell = MapManager.Instance.TilemapGameplay.WorldToCell(transform.position);
            Vector3 cellCenterPos = MapManager.Instance.TilemapGameplay.GetCellCenterWorld(cell);

            // Try to find if there is another bomb already planted.
            Collider2D[] triggerObjects = Physics2D.OverlapCircleAll(
                new Vector2(
                    cellCenterPos.x,
                    cellCenterPos.y
                ),
                MapManager.Instance.TilemapCellHalfSize,
                1 << LayerMask.NameToLayer(Constants.UserLayerNameTriggerObject)
            );
            for (int i = 0; i < triggerObjects.Length; i++)
                if (triggerObjects[i].CompareTag("Bomb"))
                    return;
            
            (Instantiate(_bombPrefab, cellCenterPos, Quaternion.identity) as GameObject).GetComponent<Bomb>().Caster = this;
            BombDeployCounter++;
            
            Debug.unityLogger.LogFormat(LogType.Log, "[{0}] Bomb planted!", Name);
        }

        /// <summary>
        /// Use special item from the player's inventory at 1st position.
        /// </summary>
        protected void UseSpecialItem()
        {
            if (SpecialItemList.Count == 0)
                return;
            
            StatusEffectManager.Instance.ApplyStatusEffect(this, null, SpecialItemList.Last());
            PlayerManagerReference.PlayerPanelReference.PlayerInventory.RemoveItemAtFirstPos(this);
        }

        /// <summary>
        /// Drop fragments when the player is killed.
        /// </summary>
        protected void DropFragments()
        {
            if (FragmentCounter == 0)
                return;
            
            Vector3Int cell = MapManager.Instance.TilemapGameplay.WorldToCell(transform.position);
            Vector3 cellCenterPos = MapManager.Instance.TilemapGameplay.GetCellCenterWorld(cell);

            // TODO: There is no check if any fragments are already on the position.
            Fragment frag = Instantiate(_fragmentPrefab, cellCenterPos, Quaternion.identity).GetComponent<Fragment>();
            frag.Quantity = FragmentCounter;

            FragmentCounter = 0;
        }

        public override void Kill(Character attacker)
        {
            if (!IsKillable() || !attacker)
                return;

            IsDeath = true;
            
            if (IsRespawnable)
                SpawnManager.Instance.RespawnCharacterInit(this, RespawnDeathDelay, MapManager.Instance.PlayerSpawnPoints);

            DropFragments();

            gameObject.SetActive(false);
            Debug.unityLogger.LogFormat(LogType.Log, "[{0}] player has been killed by character: [{1}]!", Name, attacker.Name);
        }

        public override void ForceKill(bool respawn)
        {
            if (!respawn)
                IsRespawnable = false;
            
            IsDeath = true;

            DropFragments();
            
            gameObject.SetActive(false);
            Debug.unityLogger.LogFormat(LogType.Log, "[{0}] player has been force killed!", Name);
        }
    }
}