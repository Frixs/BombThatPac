using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace UI.Intro
{
    public class IntroController : MonoBehaviour
    {
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

        private void StartIntro()
        {
            _videoPlayer.Play();
            
            Invoke("LoadTheGame", (float) _videoPlayer.clip.length);
        }

        private void LoadTheGame()
        {
            SceneLoadingManager.Instance.LoadScene(1);
            
            // Set the background color of the loading screen to black.
            GameObject.Find("LoadingScreen").GetComponent<Image>().sprite = null;
            GameObject.Find("LoadingScreen").GetComponent<Image>().color = Color.black;
        }
    }
}