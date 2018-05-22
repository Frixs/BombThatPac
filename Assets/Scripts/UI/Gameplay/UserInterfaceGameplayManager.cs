using Managers;
using UnityEngine;

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
		
		/// <summary>
		/// PlayerPanel PREFAB - Player2.
		/// </summary>
		[SerializeField] private PlayerPanel _playerPanelRotatedPrefab;
		
		/// <summary>
		/// PauseMenu reference.
		/// </summary>
		[SerializeField] private PauseMenu _pauseMenuReference;
		
		/// <summary>
		/// ScoreMenu reference.
		/// </summary>
		[SerializeField] public ScoreMenu ScoreMenuReference;
		
		/// <summary>
		/// CountdownMenu reference.
		/// </summary>
		[SerializeField] public CountdownMenu CountdownMenuReference;
		
		/// <summary>
		/// NotificationPanel reference.
		/// </summary>
		[SerializeField] public NotificationPanel NotificationPanelReference;
		
		/// <summary>
		/// This music is playing if the pause menu is open.
		/// </summary>
		[Header("Music Settings")] public AudioClip PauseModeMusic;

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
		void Start()
		{
		}
	
		// Update is called once per frame
		void Update()
		{
			if (!GameManager.Instance.IsGamePaused && !GameManager.Instance.IsInInitialCountdown && Input.GetKeyDown(KeyCode.Escape))
				PauseTheGame();
		}

		/// <summary>
		/// Instantiate PlayerPanel.
		/// </summary>
		/// <param name="rotated">Should the panel be rotated?</param>
		/// <returns>Reference of Instantiated PlayerPanel.</returns>
		public PlayerPanel InstantiatePlayerPanel(bool rotated)
		{
			if (_playerPanelPrefab == null || _playerPanelRotatedPrefab == null)
			{
				Debug.unityLogger.Log(LogType.Error, "Null reference to prefab!");
				return null;
			}

			PlayerPanel pp = Instantiate(!rotated ? _playerPanelPrefab : _playerPanelRotatedPrefab);
			pp.transform.SetParent(this.transform);

			return pp;
		}

		/// <summary>
		/// Pause the game and open the pause menu.
		/// </summary>
		private void PauseTheGame()
		{
			_pauseMenuReference.gameObject.SetActive(true);
			Time.timeScale = 0f;
			GameManager.Instance.IsGamePaused = true;
			
			SoundManager.Instance.PlayNewBackgroundMusic(PauseModeMusic);
		}
	}
}
