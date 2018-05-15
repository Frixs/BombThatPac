using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class ItemBombTrigger : MonoBehaviour
    {
        /// <summary>
        /// Bomb collider.
        /// </summary>
        [HideInInspector] public Collider2D BombCollider;

        /// <summary>
        /// Collider of character.
        /// </summary>
        [HideInInspector] public List<Collider2D> CharacterCollider = new List<Collider2D>();
        
        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        // Restore the collision when player will be in sufficient distance from the bomb.
        private void OnTriggerExit2D(Collider2D other)
        {
            bool isOut = false;
            
            for (int i = 0; i < CharacterCollider.Count; i++)
            {
                if (other == CharacterCollider[i])
                {
                    isOut = true;
                    break;
                }
            }

            if (isOut)
            {
                for (int i = 0; i < CharacterCollider.Count; i++)
                {
                    Physics2D.IgnoreCollision(CharacterCollider[i], BombCollider, false);
                }
            }
        }
    }
}