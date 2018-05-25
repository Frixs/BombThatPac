using System;
using System.Collections;
using System.Collections.Generic;
using Cameras;
using Characters;
using Special;
using StatusEffects.Scriptable;
using UI.Gameplay;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Managers
{
    /// <summary>
    /// Initialization & controlling the game.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// Static instance of GameManager which allows it to be accessed by any other script.
        /// </summary>
        public static GameManager Instance = null;

        /// <summary>
        /// Check if the game is currently paused or not.
        /// </summary>
        [HideInInspector] public bool IsGamePaused = false;

        /// <summary>
        /// Game mode if it is local game or online game, etc.
        /// </summary>
        [Header("Game Constants")] public GameType CurrentGameType;

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
        /// The last and the highest identifier of all spawned units or objects in a whole game.
        /// </summary>
        [HideInInspector] public int MaxObjectIdentifier = 0;

        /// <summary>
        /// Initial countdown on the start game.
        /// </summary>
        private int _initialCountdown = Constants.GameStartCountdown;

        /// <summary>
        /// PREFAB of finish portal to be spawned at the end.
        /// </summary>
        [SerializeField] private GameObject _finishPortalPrefab;

        /// <summary>
        /// Reference to ghost house door.
        /// </summary>
        [SerializeField] private GhostHouseDoor _ghostHouseDoor;

        /// <summary>
        /// Reference to the CameraControl script for control during different phases.
        /// </summary>
        [Header("Camera Settings")] public GameCameraControl CameraControl;

        /// <summary>
        /// Effect which is applied on it during player has all the fragments.
        /// </summary>
        [Header("Player Settings")] [SerializeField]
        private ScriptableAuraEffect _winnerAuraEffect;

        /// <summary>
        /// Reference to the prefab the players will control.
        /// </summary>
        public GameObject[] PlayerPrefabs;

        /// <summary>
        /// A collection of managers for enabling and disabling different aspects of the tanks.
        /// </summary>
        public PlayerManager[] Players;

        /// <summary>
        /// All ghost prefabs to be spawned. Depends on the order for proper spawn position assignment!!!
        /// </summary>
        [Header("Ghost Settings")] [SerializeField]
        private Ghost[] _ghostsToSpawnPrefabs;

        /// <summary>
        /// Reference to all spawned ghosts.
        /// </summary>
        [HideInInspector] public Ghost[] Ghosts;

        /// <summary>
        /// PacMan prefab to be spawned.
        /// </summary>
        [Header("PacMan Settings")] [SerializeField]
        private PacMan _pacManToSpawnPrefab;

        /// <summary>
        /// Reference to spawned PacMan.
        /// </summary>
        [HideInInspector] public PacMan PacMan;

        /// <summary>
        /// This is the default background music on the game.
        /// </summary>
        [Header("Music Settings")] public AudioClip GameModeMusic;

        /// <summary>
        /// Score mode music background.
        /// </summary>
        public AudioClip ScoreModeMusic;

        /// <summary>
        /// This is the default frightened background music on the game.
        /// </summary>
        public AudioClip FrightenedModeMusic;

        /// <summary>
        /// Initialization SFX for frightened mode.
        /// </summary>
        public AudioClip FrightenedModeMusicInitSfx;

        /// <summary>
        /// End SFX for frightened mode.
        /// </summary>
        public AudioClip FrightenedModeMusicEndSfx;

        /// <summary>
        /// Check if game is currently in frightened mode.
        /// </summary>
        [HideInInspector] public bool IsFrightenedModeUp;

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

        /// <summary>
        /// Checks if any of player has all fragments.
        /// </summary>
        [HideInInspector] public bool HasAnyPlayerAllFragments = false;

        /// <summary>
        /// Possible player winner. Player with all fragments.
        /// </summary>
        [HideInInspector] public Player PossiblePlayerWinner;

        /// <summary>
        /// Check if the game is currently in initial game countdown.
        /// </summary>
        [HideInInspector] public bool IsInInitialCountdown = false;

        /// <summary>
        /// Currently opened finish portal reference.
        /// </summary>
        private FinishPortal _openedFinishPortal;

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
        void Start()
        {
            // Create the delays so they only have to be made once.
            _startWait = new WaitForSeconds(StartDelay);
            _endWait = new WaitForSeconds(EndDelay);

            SpawnAllUnits();

            // Start playing game music loop.
            SoundManager.Instance.PlayNewBackgroundMusic(GameModeMusic);

            StartInitialCountdown();
        }

        // Update is called once per frame
        void Update()
        {
            CheckPlayerFragmentCount();
        }

        /// <summary>
        /// Check all player fragment count and open the finish portal if it is possible.
        /// TODO: This method is probably not on the correct place.
        /// </summary>
        private void CheckPlayerFragmentCount()
        {
            HasAnyPlayerAllFragments = false;

            // Go through all players.
            for (int i = 0; i < Players.Length; i++)
            {
                if (Players[i] == null || Players[i].PlayerComponent == null)
                    continue;

                if (Players[i].PlayerComponent.FragmentCounter >= MapManager.Instance.TotalFragmentCount)
                {
                    // Show notification.
                    if (PossiblePlayerWinner == null || PossiblePlayerWinner.Identifier != Players[i].PlayerComponent.Identifier)
                    {
                        UserInterfaceGameplayManager.Instance.NotificationPanelReference.ShowNotification("PACMAN IS WAITING FOR YOU, " + Players[i].PlayerComponent.Name.ToUpper() + "!");

                        // Apply winner aura effect.
                        StatusEffectManager.Instance.ApplyStatusEffect(Players[i].PlayerComponent, null, _winnerAuraEffect);
                    }

                    HasAnyPlayerAllFragments = true;
                    
                    PossiblePlayerWinner = Players[i].PlayerComponent;
                    SetWinnerPossibility();
                    
                    break;
                }
            }

            // Unless any of players have all fragments, destroy portal. 
            if (!HasAnyPlayerAllFragments && _openedFinishPortal != null)
            {
                // Destroy the portal.
                Destroy(_openedFinishPortal.gameObject);
                _openedFinishPortal = null;
                
                UnsetWinnerPossibility();
                PossiblePlayerWinner = null;
            }
        }

        /// <summary>
        /// Set the player possibility to win the game.
        /// </summary>
        private void SetWinnerPossibility()
        {
            int finishSpawnPointId = Random.Range(0, MapManager.Instance.FinishSpawnPoints.Length);
            if (_openedFinishPortal == null)
            {
                _openedFinishPortal = Instantiate(_finishPortalPrefab, MapManager.Instance.FinishSpawnPoints[finishSpawnPointId].transform.position, Quaternion.identity).GetComponent<FinishPortal>();
                
                // Open the door.
                List<Collider2D> colliders = new List<Collider2D>();
                colliders.Add(PossiblePlayerWinner.MyCollider);
                colliders.Add(PossiblePlayerWinner.MyOutsideCollider);
                _ghostHouseDoor.OpenDoor(colliders);
            }
        }

        /// <summary>
        /// Unset player possibility to win the game.
        /// </summary>
        private void UnsetWinnerPossibility()
        {
            // Close the door.
            List<Collider2D> colliders = new List<Collider2D>();
            colliders.Add(PossiblePlayerWinner.MyCollider);
            colliders.Add(PossiblePlayerWinner.MyOutsideCollider);
            _ghostHouseDoor.CloseDoor(colliders);
        }

        /// <summary>
        /// Spawn all units.
        /// </summary>
        public void SpawnAllUnits()
        {
            SpawnAllPlayers();
            SetCameraTargets();
            SpawnAllGhosts();
            SpawnPacMan();

            Time.timeScale = 1f;
        }

        /// <summary>
        /// Depspawn all units.
        /// </summary>
        public void DespawnAllUnits()
        {
            Time.timeScale = 0f;

            DespawnAllPlayers();
            DespawnAllGhosts();
            DespawnPacMan();
        }

        /// <summary>
        /// Respawn all units as new.
        /// </summary>
        public void RespawnAllUnits()
        {
            DespawnAllUnits();
            SpawnAllUnits();
        }

        /// <summary>
        /// Spawn all players with choosen role.
        /// </summary>
        private void SpawnAllPlayers()
        {
            // For all the players.
            for (int i = 0; i < Players.Length; i++)
            {
                // ... create them, set their player number and references needed for control.
                Players[i].CharacterInstance = Instantiate(PlayerPrefabs[0], MapManager.Instance.PlayerSpawnPoints[i].position, MapManager.Instance.PlayerSpawnPoints[i].rotation);
                Players[i].PlayerComponent = Players[i].CharacterInstance.GetComponent<Player>();
                Players[i].PlayerComponent.Identifier = ++MaxObjectIdentifier;
                Players[i].PlayerComponent.Name = "Player" + Players[i].PlayerComponent.Identifier;
                Players[i].Setup(i);

                // Spawn animation.
                SpawnManager.Instance.SpawnAnimationAtPositionWithExpiry(Players[i].PlayerComponent.InitSpawnAnimPrefab, MapManager.Instance.PlayerSpawnPoints[i].position, Quaternion.identity);
            }
        }

        /// <summary>
        /// Despawn all players.
        /// </summary>
        private void DespawnAllPlayers()
        {
            // For all the players.
            for (int i = 0; i < Players.Length; i++)
            {
                Players[i].Deinitialize();
            }
        }

        /// <summary>
        /// Spawn all ghosts to its positions.
        /// </summary>
        private void SpawnAllGhosts()
        {
            Transform ghostStartTargetPoint = GameObject.Find("GhostStartTargetPositionPoint").transform;

            for (int i = 0; i < _ghostsToSpawnPrefabs.Length; i++)
            {
                Transform spawnPoint = GameObject.Find("Ghost" + _ghostsToSpawnPrefabs[i].GetType().Name + "SpawnPoint").transform;
                Transform scatterBasePoint = GameObject.Find("Ghost" + _ghostsToSpawnPrefabs[i].GetType().Name + "ScatterBase").transform;

                Ghosts[i] = Instantiate(_ghostsToSpawnPrefabs[i], spawnPoint.position, _ghostsToSpawnPrefabs[i].transform.rotation);
                Ghosts[i].Identifier = ++MaxObjectIdentifier;
                Ghosts[i].Name = "Ghost" + _ghostsToSpawnPrefabs[i].GetType().Name;
                Ghosts[i].SpawnPosition = spawnPoint;
                Ghosts[i].ScatterBasePosition = scatterBasePoint;
                Ghosts[i].StartTargetPosition = ghostStartTargetPoint;
            }
        }

        /// <summary>
        /// Despawn all ghosts.
        /// </summary>
        private void DespawnAllGhosts()
        {
            for (int i = 0; i < _ghostsToSpawnPrefabs.Length; i++)
            {
                Destroy(Ghosts[i].gameObject);
                Ghosts[i] = null;
            }
        }

        /// <summary>
        /// Spawn PacMan to its position.
        /// </summary>
        private void SpawnPacMan()
        {
            Transform spawnPoint = GameObject.Find("PacManSpawnPoint").transform;

            PacMan = Instantiate(_pacManToSpawnPrefab, spawnPoint.position, _pacManToSpawnPrefab.transform.rotation);
            PacMan.Identifier = ++MaxObjectIdentifier;
            PacMan.Name = "PacMan";
        }

        /// <summary>
        /// Despawn PacMan.
        /// </summary>
        private void DespawnPacMan()
        {
            Destroy(PacMan.gameObject);
            PacMan = null;
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
                targets[i] = Players[i].CharacterInstance.transform;
            }

            // These are the targets the camera should follow.
            CameraControl.Targets = targets;
        }

        /// <summary>
        /// End the game and show scoreboard with winner player statistics.
        /// </summary>
        public void EndTheGame()
        {
            Time.timeScale = 0f;
            IsGamePaused = true;
            UserInterfaceGameplayManager.Instance.ScoreMenuReference.gameObject.SetActive(true);
            UserInterfaceGameplayManager.Instance.ScoreMenuReference.PlacerNamePlaceholderText.text = "Player " + PossiblePlayerWinner.Identifier;
            UserInterfaceGameplayManager.Instance.NotificationPanelReference.ForceOffNotification();

            SoundManager.Instance.PlayNewBackgroundMusic(ScoreModeMusic);

            for (int i = 0; i < Players.Length; i++)
            {
                Destroy(Players[i].PlayerPanelReference.gameObject);
                Players[i].PlayerPanelReference = null;
            }
        }

        /// <summary>
        /// Start initial coundown of the game.
        /// </summary>
        private void StartInitialCountdown()
        {
            IsInInitialCountdown = true;

            StartCoroutine(InitialCountdownProgress(_initialCountdown));
        }

        /// <summary>
        /// Progress the countdown during time is stopped.
        /// </summary>
        /// <param name="delay">Delay of the countdown.</param>
        /// <returns>IEnumerator.</returns>
        private IEnumerator InitialCountdownProgress(int delay)
        {
            const float howManyTimesCountdownShouldBeSlowed = 1.5f;

            IsGamePaused = true;
            Time.timeScale = 0f;

            float countdownTimer = 0f;
            float previousTime = Time.realtimeSinceStartup;

            while (countdownTimer <= delay)
            {
                countdownTimer += (Time.realtimeSinceStartup - previousTime) / howManyTimesCountdownShouldBeSlowed;
                previousTime = Time.realtimeSinceStartup;

                int countdown = (int) Math.Round(delay - countdownTimer);

                string textLabel = "";

                if (countdown == delay)
                    textLabel += "GET READY";
                else if (countdown > 0)
                    textLabel += countdown;
                else
                    textLabel += "START";

                // Countdown text label.
                UserInterfaceGameplayManager.Instance.CountdownMenuReference.CountdownText.text = textLabel;
                // Background alpha stairs.
                UserInterfaceGameplayManager.Instance.CountdownMenuReference.GetComponent<Image>().color = new Color(
                    UserInterfaceGameplayManager.Instance.CountdownMenuReference.GetComponent<Image>().color.r,
                    UserInterfaceGameplayManager.Instance.CountdownMenuReference.GetComponent<Image>().color.g,
                    UserInterfaceGameplayManager.Instance.CountdownMenuReference.GetComponent<Image>().color.b,
                    countdown * (0.25f / (delay + 0.5f)) + 0.75f
                );

                yield return 0;
            }

            UserInterfaceGameplayManager.Instance.CountdownMenuReference.gameObject.SetActive(false);
            Time.timeScale = 1;
            IsInInitialCountdown = false;
            IsGamePaused = false;

            // Show goal message.
            yield return new WaitForSeconds(Constants.GoalNotificationDelay);
            UserInterfaceGameplayManager.Instance.NotificationPanelReference.ShowNotification("COLLECT ALL THE COINS!");
        }
    }

    public enum GameType
    {
        Local
    }
}