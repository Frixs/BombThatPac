using Managers;
using UnityEngine;

namespace UI.Gameplay
{
    public class PauseMenu : MonoBehaviour
    {
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
        
        public void QuitButtonEvent()
        {
            Application.Quit();
        }
    }
}