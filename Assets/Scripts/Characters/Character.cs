using UnityEngine;
using UnityEngine.Tilemaps;

namespace Characters
{
    /// <summary>
    /// This class defines the basics of all characters in the game.
    /// </summary>
    public abstract class Character : MonoBehaviour
    {
        /// <summary>
        /// Name of the current character.
        /// </summary>
        [SerializeField] private string _name = Constants.CharacterDefaultName;

        public string Name
        {
            get { return _name; }
        }

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

        /// <summary>
        /// Moves the player.
        /// </summary>
        public void Move()
        {
            // Makes sure that the player moves.
            _myRigidBody.velocity = Direction.normalized * _speed;
        }

        /// <summary>
        /// TODO
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
        /// TODO
        /// </summary>
        private void ActivateLayer(string layerName)
        {
            for (int i = 0; i < _myAnimator.layerCount; i++)
            {
                _myAnimator.SetLayerWeight(i, 0);
            }

            _myAnimator.SetLayerWeight(_myAnimator.GetLayerIndex(layerName), 1);
        }
    }
}