using System.Collections.Generic;
using Managers;
using StatusEffects;
using UnityEngine;

namespace Characters
{
    /// <summary>
    /// This class defines the basics of all characters in the game.
    /// </summary>
    public abstract class Character : MonoBehaviour
    {
        /// <summary>
        /// The Character's movement speed.
        /// </summary>
        public abstract float MoveSpeed { get; set; }
        
        /// <summary>
        /// Delay to respawn.
        /// </summary>
        public abstract float RespawnDeathDelay { get; set; }
        
        /// <summary>
        /// Reference to event animation controller.
        /// </summary>
        public abstract RuntimeAnimatorController EventAnimationController { get; }
        
        /// <summary>
        /// Used to identify which tank belongs to which player.
        /// </summary>
        [HideInInspector] public int Identifier;
        
        /// <summary>
        /// Name of the current character.
        /// </summary>
        [HideInInspector] public string Name = Constants.CharacterDefaultName;
        
        /// <summary>
        /// All status effects which are currently applied on the character.
        /// </summary>
        public List<StatusEffect> AppliedStatusEffects = new List<StatusEffect>();

        /// <summary>
        /// The Character's direction. This value should be defined in inherit class in method like GetInput (Player).
        /// Current direction with memory of the last other direction.
        /// </summary>
        protected Vector2 Direction, PreviousDirection = Vector2.zero;

        /// <summary>
        /// Reference to default animation controller. If character changes its animations we know which is the default one to go back.
        /// </summary>
        [HideInInspector] public RuntimeAnimatorController AnimationControllerDefault;

        /// <summary>
        /// A reference to the character's animator.
        /// </summary>
        [HideInInspector] public Animator MyAnimator;
        
        /// <summary>
        /// Reference to rigid body.
        /// </summary>
        protected Rigidbody2D MyRigidBody;

        /// <summary>
        /// Is character invulnerable thanks to some effect?
        /// </summary>
        [HideInInspector] public bool IsInvulnearable = false;
        
        /// <summary>
        /// Is character immute to apply any status effect?
        /// </summary>
        [HideInInspector] public bool IsStatusEffectImmune = false;
        
        /// <summary>
        /// Check if the character is already death or not.
        /// </summary>
        [HideInInspector] public bool IsDeath;
        
        /// <summary>
        /// Tells if character can respawn after death.
        /// </summary>
        [HideInInspector] public bool IsRespawnable = true;

        /// <summary>
        /// Check if character is in some event animation.
        /// </summary>
        private bool _isEventAnimation = false;

        /// <summary>
        /// Event animation ID. This ID is defined in BlendTree in Animator event controller.
        /// </summary>
        private int _eventAnimationId;

        /// <summary>
        /// Timer for event action animation.
        /// </summary>
        private float _eventAnimationTimer;
        
        /// <summary>
        /// Set if character can move and interact with other characters.
        /// 
        /// FALSE:
        /// Character cannot move.
        /// Players cannot use inputs.
        /// AI cannot attack.
        /// </summary>
        public bool HasEnabledActions { get; protected set; } = true;

        // Use this for initialization
        protected virtual void Start()
        {
            MyRigidBody = GetComponent<Rigidbody2D>();
            MyAnimator = GetComponent<Animator>();
            AnimationControllerDefault = MyAnimator.runtimeAnimatorController;
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            // Handle animation layers and set the correct one.
            HandleAnimationLayers();

            // Process all character's buffs and debuffs.
            StatusEffectManager.Instance.ProcessStatusEffects(this);
        }

        // Fixed update
        protected virtual void FixedUpdate()
        {
            Move();
        }

        // On script enables.
        private void OnEnable()
        {
            IsDeath = false;
        }
        
        /// <summary>
        /// Check if character is moving.
        /// </summary>
        public bool IsMoving()
        {
            return Direction.x != 0 || Direction.y != 0;
        }
        
        /// <summary>
        /// Moves the character.
        /// </summary>
        public abstract void Move();

        /// <summary>
        /// Get orientation of the character. Normalized vector with only 1 possible direction (W, A, S, D).
        /// </summary>
        /// <returns>Orientation.</returns>
        public abstract Vector2 GetOrientation();

        /// <summary>
        /// Kill the character.
        /// </summary>
        /// <param name="attacker">Reference to attacker character.</param>
        public abstract void Kill(Character attacker);

        /// <summary>
        /// Immediately kills the character without any reason.
        /// </summary>
        /// <param name="respawn">TRUE for respawn after death. FALSE for no respawn anymore.</param>
        public abstract void ForceKill(bool respawn);

        /// <summary>
        /// Check if character is killable.
        /// </summary>
        /// <returns>TRUE: Is killable. FALSE: Character is NOT killable.</returns>
        public bool IsKillable()
        {
            return !IsDeath && !IsInvulnearable;
        }

        /// <summary>
        /// Handle animation layers.
        /// </summary>
        protected virtual void HandleAnimationLayers()
        {
            // Special event action animations.
            if (_isEventAnimation)
            {
                // Check if any event animation is already in progress.
                if (MyAnimator.runtimeAnimatorController == EventAnimationController)
                {
                    if ((int) MyAnimator.GetFloat("event_id") != _eventAnimationId)
                        _eventAnimationTimer = 0;
                }
                // If no evnet animation is not executing right now, change controller to event controller.
                else
                    MyAnimator.runtimeAnimatorController = EventAnimationController;
                
                // Timer of the event animation.
                _eventAnimationTimer += Time.deltaTime;
                
                // Tell to controller, which animation should be executed.
                MyAnimator.SetFloat("event_id", _eventAnimationId);

                // Timing the end of animation.
                if (_eventAnimationTimer >= MyAnimator.runtimeAnimatorController.animationClips[_eventAnimationId - 1].length)
                {
                    _isEventAnimation = false;
                    _eventAnimationId = 0;
                    _eventAnimationTimer = 0;
                }
                else
                    return;
            }

            // Set default move animation controller.
            if (MyAnimator.runtimeAnimatorController != AnimationControllerDefault)
                MyAnimator.runtimeAnimatorController = AnimationControllerDefault;
            
            // Checks if we are moving or standing still.
            if (IsMoving())
            {
                ActivateLayer("WalkLayer");

                // Sets the animation parameter so that he faces the correct direction.
                MyAnimator.SetFloat("x", Direction.x);
                MyAnimator.SetFloat("y", Direction.y);
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
            for (int i = 0; i < MyAnimator.layerCount; i++)
            {
                MyAnimator.SetLayerWeight(i, 0);
            }

            MyAnimator.SetLayerWeight(MyAnimator.GetLayerIndex(layerName), 1);
        }

        /// <summary>
        /// Set execution for event action animation.
        /// </summary>
        /// <param name="id">ID of the animation (It is defined in its Controller in BlendTree).</param>
        public void StartEventAnimation(int id)
        {
            if (id <= 0 || id >= EventAnimationController.animationClips.Length)
            {
                Debug.unityLogger.Log(LogType.Error, "Animation event action is out of range!");
                return;
            }
            
            _isEventAnimation = true;
            _eventAnimationId = id;
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