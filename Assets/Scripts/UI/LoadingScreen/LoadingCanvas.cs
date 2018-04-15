using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.LoadingScreen
{
    public class LoadingCanvas : MonoBehaviour
    {
        /// <summary>
        /// Reference to loading screen panel.
        /// </summary>
        public GameObject LoadingScreen;

        /// <summary>
        /// Reference to loading screen slider in loading screen panel.
        /// </summary>
        public Slider LoadingSlider;

        /// <summary>
        /// Reference to slider's progress text label.
        /// </summary>
        public TMP_Text LoadingProgressText;
    }
}