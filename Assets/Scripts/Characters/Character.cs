using UnityEngine;

namespace Characters
{
    /// <summary>
    /// This class defines the basics of all characters in the game.
    /// </summary>
    public abstract class Character : MonoBehaviour
    {
        /// <summary>
        /// Used to identify which tank belongs to which player.
        /// </summary>
        [HideInInspector] public int PlayerNumber;
        
        /// <summary>
        /// Name of the current character.
        /// </summary>
        [HideInInspector] public string Name = Constants.CharacterDefaultName;

        /// <summary>
        /// The Player's movement speed.
        /// </summary>
        [SerializeField] private float _speed = Constants.PlayerDefaultSpeed;

        /// <summary>
        /// The Player's direction.
        /// </summary>
        protected Vector2 Direction;

        /// <summary>
        /// A reference to the character's animator.
        /// </summary>
        private Animator _myAnimator;

        /// <summary>
        /// Reference to rigid body.
        /// </summary>
        private Rigidbody2D _myRigidBody;

        /// <summary>
        /// Is character invulnerable thanks to some effect?
        /// </summary>
        [HideInInspector] public bool IsInvulnearable = false;
        
        /// <summary>
        /// Check if the character is already death or not.
        /// </summary>
        private bool _isDeath;
        
        /// <summary>
        /// Set if character can move and interact with other characters.
        /// 
        /// FALSE:
        /// Character cannot move.
        /// Players cannot use inputs.
        /// AI cannot attack.
        /// </summary>
        public bool HasEnabledActions { get; private set; } = true;

        // Use this for initialization
        protected virtual void Start()
        {
            _myRigidBody = GetComponent<Rigidbody2D>();
            _myAnimator = GetComponent<Animator>();
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            HandleLayers();
        }
        
        /// <summary>
        /// Check if character is moving.
        /// </summary>
        public bool IsMoving()
        {
            return Direction.x != 0 || Direction.y != 0;
        }

        // Fixed update
        private void FixedUpdate()
        {
            Move();
        }

        // On script enables.
        private void OnEnable()
        {
            _isDeath = false;
        }

        /// <summary>
        /// Moves the player.
        /// </summary>
        public void Move()
        {
            // Makes sure that the player moves.
            _myRigidBody.velocity = Direction.normalized * _speed * (HasEnabledActions ? 1 : 0);
        }

        /// <summary>
        /// Handle animation layers.
        /// </summary>
        private void HandleLayers()
        {
            // Checks if we are moving or standing still.
            if (IsMoving())
            {
                ActivateLayer("WalkLayer");

                // Sets the animation parameter so that he faces the correct direction.
                _myAnimator.SetFloat("x", Direction.x);
                _myAnimator.SetFloat("y", Direction.y);
            }
            else
            {
                ActivateLayer("IdleLayer");
            }
        }

        /// <summary>
        /// Activate current animation layer.
        /// </summary>
        private void ActivateLayer(string layerName)
        {
            for (int i = 0; i < _myAnimator.layerCount; i++)
            {
                _myAnimator.SetLayerWeight(i, 0);
            }

            _myAnimator.SetLayerWeight(_myAnimator.GetLayerIndex(layerName), 1);
        }

        /// <summary>
        /// Check if character is killable.
        /// </summary>
        /// <returns>TRUE: Is killable. FALSE: Character is NOT killable.</returns>
        public bool IsKillable()
        {
            return !_isDeath && !IsInvulnearable;
        }
        
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="attacker"></param>
        public void Kill(Character attacker)
        {
            // TODO We neeed to determiny if it is Player or AI. Each of htem has other procedures to do (like score counting).
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="respawn"></param>
        public void ForceKill(bool respawn)
        {
            
        }
        
        /// <summary>
        /// Enable actions.
        /// </summary>
        public void EnableActions()
        {
            HasEnabledActions = true;
        }
        
        /// <summary>
        /// Disable actions.
        /// </summary>
        public void DisableActions()
        {
            HasEnabledActions = false;
        }
    }
}