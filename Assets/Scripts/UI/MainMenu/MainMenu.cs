using UnityEngine;

namespace UI.MainMenu
{
    public class MainMenu : MonoBehaviour
    {
        /// <summary>
        /// Button event method.
        /// </summary>
        public void QuitButtonEvent()
        {
            Application.Quit();
        }
    }
}