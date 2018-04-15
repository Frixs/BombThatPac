using System.Collections;
using UI.LoadingScreen;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class SceneLoadingManager : MonoBehaviour
    {
        /// <summary>
        /// Static instance of SceneLoadingManager which allows it to be accessed by any other script.
        /// </summary>
        public static SceneLoadingManager Instance = null;
        
        /// <summary>
        /// PREFAB of loading canvas.
        /// </summary>
        [SerializeField] private GameObject _loadingCanvasPrefab;

        /// <summary>
        /// Minimal loading time to be spent in the loading screen.
        /// </summary>
        [SerializeField] private float _minLoadingTime;
        
        /// <summary>
        /// Timer for minimal time spent in the loading screen.
        /// </summary>
        private float _minLoadingTimer;
        
        /// <summary>
        /// Reference to loading canvas in the current scene.
        /// </summary>
        private LoadingCanvas _loadingCanvas;
        
        // Awake is always called before any Start functions
        void Awake()
        {
            // Check if instance already exists.
            if (Instance == null)
            {
                // If not, set instance to this.F
                Instance = this;
            }
            // If instance already exists and it's not this.
            else if (Instance != this)
            {
                // Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a SceneLoadingManager.
                Destroy(gameObject);
            }

            // Sets this to not be destroyed when reloading scene.
            //DontDestroyOnLoad(gameObject);
        }
        
        /// <summary>
        /// Reload scene to another one with loading screen.
        /// </summary>
        /// <param name="sceneIndex">Scene index to be loaded.</param>
        public void LoadScene(int sceneIndex)
        {
            if (_loadingCanvas == null)
                _loadingCanvas = Instantiate(_loadingCanvasPrefab, transform.position, Quaternion.identity).GetComponent<LoadingCanvas>();
            
            StartCoroutine(LoadAsynchronously(sceneIndex));
        }

        /// <summary>
        /// Coroutine to asynchronously load new scene.
        /// </summary>
        /// <param name="sceneIndex">Scene index to be loaded.</param>
        /// <returns></returns>
        private IEnumerator LoadAsynchronously(int sceneIndex)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
            operation.allowSceneActivation = false;

            _loadingCanvas.LoadingScreen.SetActive(true);

            bool isDone = false;
            while (!isDone)
            {
                // Timer for minimal spent time.
                _minLoadingTimer += Time.deltaTime > 0 ? Time.deltaTime : 0.01f; // If the game is paused, use the special const defined only here.
                
                float progress = Mathf.Clamp01(
                    (((_minLoadingTimer / (_minLoadingTime / 100f)) / 100f) + (operation.progress / .9f)) / 2f
                );

                _loadingCanvas.LoadingSlider.value = progress;
                _loadingCanvas.LoadingProgressText.text = Mathf.RoundToInt(progress * 100f) + "%";

                if (_minLoadingTimer >= _minLoadingTime)
                {
                    operation.allowSceneActivation = true;
                    
                    if (operation.isDone)
                        isDone = true;
                }

                yield return null;
            }
        }
    }
}