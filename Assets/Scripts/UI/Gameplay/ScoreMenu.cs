using Managers;
using TMPro;
using UnityEngine;

namespace UI.Gameplay
{
    public class ScoreMenu : MonoBehaviour
    {
        /// <summary>
        /// Player name placeholder text reference.
        /// </summary>
        [SerializeField] public TMP_Text PlacerNamePlaceholderText;
        
        public void MainMenuButtonEvent()
        {
            gameObject.SetActive(false);
            SceneLoadingManager.Instance.LoadScene(0);
        }
        
        public void QuitButtonEvent()
        {
            Application.Quit();
        }
    }
}