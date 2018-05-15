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
        [HideInInspector] public List<Collider2D> CharacterColliderList;

        private void Awake()
        {
            CharacterColliderList = new List<Collider2D>();
        }

        // Restore the collision when player will be in sufficient distance from the bomb.
        private void OnTriggerExit2D(Collider2D other)
        {
            bool isOut = false;
            
            for (int i = 0; i < CharacterColliderList.Count; i++)
            {
                if (other == CharacterColliderList[i])
                {
                    isOut = true;
                    break;
                }
            }

            if (isOut)
            {
                for (int i = 0; i < CharacterColliderList.Count; i++)
                {
                    Physics2D.IgnoreCollision(CharacterColliderList[i], BombCollider, false);
                }
            }
        }
    }
}