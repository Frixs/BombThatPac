using Camera;
using Characters;
using UnityEngine;

namespace Managers
{
	public class GameManager : MonoBehaviour
	{
		/// <summary>
		/// Static instance of GameManager which allows it to be accessed by any other script.
		/// </summary>
		public static GameManager Instance = null;

		/// <summary>
		/// Game mode if it is local game or online game, etc.
		/// </summary>
		public string GameType = "LOCAL";
		
		/// <summary>
		/// The number of rounds a single player has to win to win the game.
		/// </summary>
		public int NumRoundsToWin = 5;
		
		/// <summary>
		/// The delay between the start of RoundStarting and RoundPlaying phases.
		/// </summary>
		public float StartDelay = 3.0f;
		
		/// <summary>
		/// // The delay between the end of RoundPlaying and RoundEnding phases.
		/// </summary>
		public float EndDelay = 3f;
		
		/// <summary>
		/// Reference to the CameraControl script for control during different phases.
		/// </summary>
		public CameraControl CameraControl;

		/// <summary>
		/// Reference to the prefab the players will control.
		/// </summary>
		public GameObject[] PlayerPrefabs;
		
		/// <summary>
		/// A collection of managers for enabling and disabling different aspects of the tanks.
		/// </summary>
		public PlayerManager[] Players;
		
		/// <summary>
		/// Which round the game is currently on.
		/// </summary>
		private int _roundNumber;
		
		/// <summary>
		/// Used to have a delay whilst the round starts.
		/// </summary>
		private WaitForSeconds _startWait;
		
		/// <summary>
		/// Used to have a delay whilst the round or game ends.
		/// </summary>
		private WaitForSeconds _endWait;
		
		/// <summary>
		/// Reference to the winner of the current round.  Used to make an announcement of who won.
		/// </summary>
		private PlayerManager _roundWinner;
		
		/// <summary>
		/// Reference to the winner of the game.  Used to make an announcement of who won.
		/// </summary>
		private PlayerManager _gameWinner;
		
		// Awake is always called before any Start functions
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
				// Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
				Destroy(gameObject);
			}

			// Sets this to not be destroyed when reloading scene.
			//DontDestroyOnLoad(gameObject);
		}

		// Use this for initialization
		void Start ()
		{
			// Create the delays so they only have to be made once.
			_startWait = new WaitForSeconds (StartDelay);
			_endWait = new WaitForSeconds (EndDelay);
			
			SpawnAllPlayers();
			SetCameraTargets();
			
			// Once the players have been created and the camera is using them as targets, start the game.
			//StartCoroutine(GameLoop());
		}
		
		/// <summary>
		/// Spawn all players with choosen role.
		/// </summary>
		private void SpawnAllPlayers()
		{
			// For all the players...
			for (int i = 0; i < Players.Length; i++)
			{
				// ... create them, set their player number and references needed for control.
				Players[i].Instance = Instantiate(PlayerPrefabs[0], Players[i].SpawnPoint.position, Players[i].SpawnPoint.rotation);
				Players[i].Player 	= Players[i].Instance.GetComponent<Player>();
				Players[i].Player.PlayerNumber = i + 1;
				Players[i].Setup();
			}
		}

		/// <summary>
		/// Set all camera targets which camera should follow.
		/// </summary>
		private void SetCameraTargets()
		{
			// Create a collection of transforms the same size as the number of tanks.
			Transform[] targets = new Transform[Players.Length];

			// For each of these transforms...
			for (int i = 0; i < targets.Length; i++)
			{
				// ... set it to the appropriate player transform.
				targets[i] = Players[i].Instance.transform;
			}

			// These are the targets the camera should follow.
			CameraControl.Targets = targets;
		}
	}
}
