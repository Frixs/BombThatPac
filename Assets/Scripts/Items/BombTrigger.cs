using UnityEngine;

namespace Items
{
    public class BombTrigger : MonoBehaviour
    {
        /// <summary>
        /// Bomb collider.
        /// </summary>
        [HideInInspector] public Collider2D BombCollider;

        /// <summary>
        /// Collider of character.
        /// </summary>
        [HideInInspector] public Collider2D CharacterCollider; 
        
        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            // Restore the collision when player will be in sufficient distance from the bomb.
            if (other == CharacterCollider)
            {
                Physics2D.IgnoreCollision(CharacterCollider, BombCollider, false);
            }
        }
    }
}