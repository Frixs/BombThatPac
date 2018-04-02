﻿using UnityEngine;

namespace UI.Gameplay
{
	public class UserInterfaceGameplayManager : MonoBehaviour
	{
		/// <summary>
		/// Static instance of UserInterfaceGameplayManager which allows it to be accessed by any other script.
		/// </summary>
		public static UserInterfaceGameplayManager Instance = null;

		/// <summary>
		/// PlayerPanel PREFAB.
		/// </summary>
		[SerializeField] private PlayerPanel _playerPanelPrefab;

		void Awake()
		{
			// Check if instance already exists.
			if (Instance == null)
			{
				// If not, set instance to this.
				Instance = this;
			}
			// If instance already exists and it's not this.
			else if (Instance != this)
			{
				// Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a UserInterfaceGameplayManager.
				Destroy(gameObject);
			}

			// Sets this to not be destroyed when reloading scene.
			//DontDestroyOnLoad(gameObject);
		}
		
		// Use this for initialization
		void Start ()
		{
		}
	
		// Update is called once per frame
		void Update ()
		{
		}

		/// <summary>
		/// Instantiate PlayerPanel.
		/// </summary>
		/// <returns>Reference of Instantiated PlayerPanel.</returns>
		public PlayerPanel InstantiatePlayerPanel()
		{
			if (_playerPanelPrefab == null)
			{
				Debug.unityLogger.Log(LogType.Error, "Null reference to prefab!");
				return null;
			}
			
			PlayerPanel pp = Instantiate(_playerPanelPrefab);
			pp.transform.SetParent(this.transform);

			return pp;
		}
	}
}