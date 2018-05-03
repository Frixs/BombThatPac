using Managers;
using UnityEngine;

/// <summary>
/// Loading all necessary instances to the game.
/// </summary>
public class Loader : MonoBehaviour
{
    /// <summary>
    /// InputManager PREFAB to instantiate.
    /// </summary>
    [Header("Manager Prefabs")] [SerializeField]
    private GameObject _inputManager;

    /// <summary>
    /// SceneLoadingManager PREFAB to instantiate.
    /// </summary>
    [SerializeField] private GameObject _sceneLoadingManager;

    /// <summary>
    /// SoundManager PREFAB to instantiate.
    /// </summary>
    [SerializeField] private GameObject _soundManager;

    // Awake is always called before any Start functions
    void Awake()
    {
        // Check if a InputManager has already been assigned to static variable InputManager.instance or if it's still null.
        if (InputManager.Instance == null)
        {
            Instantiate(_inputManager);
            //go.transform.parent = _managers;
        }

        if (SceneLoadingManager.Instance == null)
        {
            Instantiate(_sceneLoadingManager);
            //go.transform.parent = _managers;
        }
        
        if (SoundManager.Instance == null)
        {
            Instantiate(_soundManager);
            //go.transform.parent = _managers;
        }
    }

    void Start()
    {
        // Be sure time is not stopped. Only if we are in the game we want to handle time with GameManager.
        if (GameManager.Instance == null)
            Time.timeScale = 1f;
    }
}