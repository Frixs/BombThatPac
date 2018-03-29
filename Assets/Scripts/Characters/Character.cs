﻿using System.Reflection;
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
        protected abstract float Speed { get; set; }
        
        /// <summary>
        /// Delay to respawn.
        /// </summary>
        public abstract float RespawnDeathDelay { get; set; }
        
        /// <summary>
        /// Used to identify which tank belongs to which player.
        /// </summary>
        [HideInInspector] public int Identifier;
        
        /// <summary>
        /// Name of the current character.
        /// </summary>
        [HideInInspector] public string Name = Constants.CharacterDefaultName;

        /// <summary>
        /// The Character's direction. This value should be defined in inherit class in method like GetInput (Player).
        /// Current direction with memory of the last other direction.
        /// </summary>
        protected Vector2 Direction, PreviousDirection = Vector2.zero;

        /// <summary>
        /// A reference to the character's animator.
        /// </summary>
        protected Animator MyAnimator;

        /// <summary>
        /// Reference to rigid body.
        /// </summary>
        protected Rigidbody2D MyRigidBody;

        /// <summary>
        /// Is character invulnerable thanks to some effect?
        /// </summary>
        [HideInInspector] public bool IsInvulnearable = false;
        
        /// <summary>
        /// Check if the character is already death or not.
        /// </summary>
        [HideInInspector] public bool IsDeath;
        
        /// <summary>
        /// Tells if character can respawn after death.
        /// </summary>
        [HideInInspector] public bool IsRespawnable = true;
        
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
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            // Handle animation layers and set the correct one.
            //HandleLayers();
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
        private void HandleLayers()
        {
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