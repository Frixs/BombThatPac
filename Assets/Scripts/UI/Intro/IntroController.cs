using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace UI.Intro
{
    public class IntroController : MonoBehaviour
    {
        /// <summary>
        /// Video player reference.
        /// </summary>
        [SerializeField] private VideoPlayer _videoPlayer;
        
        // Use this for initialization
        void Start()
        {
            Invoke("StartIntro", 1f);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.anyKeyDown)
            {
                _videoPlayer.Stop();
                LoadTheGame();
            }
        }

        /// <summary>
        /// Start playing the intro video.
        /// </summary>
        private void StartIntro()
        {
            _videoPlayer.Play();
            
            Invoke("LoadTheGame", (float) _videoPlayer.clip.length);
        }

        /// <summary>
        /// Load the game scene.
        /// Set loading screen to black background bacause to intro ending.
        /// </summary>
        private void LoadTheGame()
        {
            SceneLoadingManager.Instance.LoadScene(1);
            
            // Set the background color of the loading screen to black.
            GameObject.Find("LoadingScreen").GetComponent<Image>().sprite = null;
            GameObject.Find("LoadingScreen").GetComponent<Image>().color = Color.black;
        }
    }
}