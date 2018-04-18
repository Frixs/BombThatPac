using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay
{
    public class PauseMenu : MonoBehaviour
    {
        /// <summary>
        /// Reference to button.
        /// </summary>
        [SerializeField] private Button _controlsKeyboardButton; 
        
        public void ResumeButtonEvent()
        {
            Time.timeScale = 1f;
            GameManager.Instance.IsGamePaused = false;
            gameObject.SetActive(false);
        }
        
        public void MainMenuButtonEvent()
        {
            gameObject.SetActive(false);
            SceneLoadingManager.Instance.LoadScene(0);
        }
        
        public void ControlsButtonEvent()
        {
            _controlsKeyboardButton.gameObject.SetActive(true);
        }
        
        public void ControlsKeyboardButtonEvent()
        {
            _controlsKeyboardButton.gameObject.SetActive(false);
        }
        
        public void QuitButtonEvent()
        {
            Application.Quit();
        }
    }
}