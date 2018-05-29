using System.Collections;
using System.Collections.Generic;
using Characters;
using Managers;
using UnityEngine;
using UnityEngine.Video;

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
		/// NotificationPanel reference.
		/// </summary>
		[SerializeField] public VideoPlayer OutroPlayerReference;
		
		/// <summary>
		/// Outro background reference.
		/// </summary>
		[SerializeField] public GameObject OutroBackgroundReference;
		
		/// <summary>
		/// Decors reference.
		/// </summary>
		[SerializeField] public GameObject DecorsReference;
		
		/// <summary>
		/// This music is playing if the pause menu is open.
		/// </summary>
		[Header("Music Settings")] public AudioClip PauseModeMusic;
		
		/// <summary>
		/// Score mode music background.
		/// </summary>
		public AudioClip ScoreModeMusic;

		/// <summary>
		/// Save the id at the end to be able to show it in scoreboard.
		/// </summary>
		private int _playerWinnerId = 0;

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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="playerWinnerId"></param>
		public void EndTheGame(int playerWinnerId)
		{
			_playerWinnerId = playerWinnerId;
			
			PlayOutro();
			NotificationPanelReference.ForceOffNotification();

			SoundManager.Instance.PlayNewBackgroundMusic(ScoreModeMusic);
		}

		private void ShowScore()
		{
			ScoreMenuReference.gameObject.SetActive(true);
			ScoreMenuReference.PlacerNamePlaceholderText.text = "Player " + _playerWinnerId;
		}
		
		/// <summary>
		/// Play outro video.
		/// </summary>
		private void PlayOutro()
		{
			OutroPlayerReference.gameObject.SetActive(true);
			OutroPlayerReference.Play();
			
			StartCoroutine(StopOutro());
		}

		/// <summary>
		/// Close and stop intro after playback.
		/// </summary>
		/// <returns>IEnumerator.</returns>
		private IEnumerator StopOutro()
		{
			float countdownTimer = 0f;
			float previousTime = Time.realtimeSinceStartup;

			while (countdownTimer <= (float) OutroPlayerReference.clip.length)
			{
				countdownTimer += Time.realtimeSinceStartup - previousTime;
				previousTime = Time.realtimeSinceStartup;
				
				yield return 0;
			}

			OutroBackgroundReference.SetActive(true);
			DecorsReference.SetActive(false);
			ShowScore();
			OutroPlayerReference.Stop();
			OutroPlayerReference.gameObject.SetActive(false);

			yield return null;
		}
	}
}
