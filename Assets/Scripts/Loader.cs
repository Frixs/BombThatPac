using Managers;
using UnityEngine;

/// <summary>
/// Loading all necessary instances to the game.
/// </summary>
public class Loader : MonoBehaviour
{
	/*
	/// <summary>
	/// Managers GO holder.
	/// </summary>
	[SerializeField]
	private Transform _managers;
	*/
	
	/// <summary>
	/// InputManager PREFAB to instantiate.
	/// </summary>
	[SerializeField]
	private GameObject _inputManager; 
	
	// Awake is always called before any Start functions
	void Awake ()
	{
		// Check if a InputManager has already been assigned to static variable InputManager.instance or if it's still null.
		if (InputManager.Instance == null)
		{
			Instantiate(_inputManager);
			//go.transform.parent = _managers;
		}
	}
	
	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
	}
}
