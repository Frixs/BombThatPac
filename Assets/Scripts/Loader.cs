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
	[SerializeField]
	private GameObject _sceneLoadingManager; 
	
	// Awake is always called before any Start functions
	void Awake ()
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
	}

	void Start()
	{
		Time.timeScale = 1f;
	}
}
