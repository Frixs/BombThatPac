using Characters;
using Managers;
using UI.Gameplay;
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
                Time.timeScale = 0f;
                GameManager.Instance.IsGamePaused = true;
                UserInterfaceGameplayManager.Instance.ScoreMenuReference.gameObject.SetActive(true);
                UserInterfaceGameplayManager.Instance.ScoreMenuReference.PlacerNamePlaceholderText.text = "Player " + GameManager.Instance.PossiblePlayerWinner.Identifier;

                for (int i = 0; i < GameManager.Instance.Players.Length; i++)
                {
                    Destroy(GameManager.Instance.Players[i].PlayerPanelReference.gameObject);
                    GameManager.Instance.Players[i].PlayerPanelReference = null;
                }
            }
        }
    }
}