using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay
{
    public class PauseMenu : MonoBehaviour
    {
        /// <summary>
        /// Reference to controls menu.
        /// </summary>
        [SerializeField] private GameObject _controlsMenu;
        
        private void Update()
        {
            if (gameObject.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape))
                ResumeButtonEvent();
        }

        public void ResumeButtonEvent()
        {
            if (_controlsMenu.activeInHierarchy)
                _controlsMenu.SetActive(false);
            
            Time.timeScale = 1f;
            GameManager.Instance.IsGamePaused = false;
            gameObject.SetActive(false);
            
            SoundManager.Instance.PlayPreviousBackgroundMusic(true);
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