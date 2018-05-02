using Characters;
using Managers;
using UnityEngine;

namespace Special
{
    public class FinishPortal : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            Component component = null;
            
            if ((component = other.gameObject.GetComponent<Player>()) != null && GameManager.Instance.PossiblePlayerWinner != null && ((Player) component).Identifier == GameManager.Instance.PossiblePlayerWinner.Identifier)
            {
                GameManager.Instance.EndTheGame();
            }
        }
    }
}