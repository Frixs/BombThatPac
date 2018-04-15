using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        /// <summary>
        /// TODO
        /// </summary>
        public void PlayButtonEvent()
        {
            SceneLoadingManager.Instance.LoadScene(1);
            //SceneManager.LoadScene(1);
        }
        
        /// <summary>
        /// TODO
        /// </summary>
        public void QuitButtonEvent()
        {
            Application.Quit();
        }
    }
}