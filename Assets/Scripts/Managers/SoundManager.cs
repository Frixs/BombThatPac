using Others;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// TODO: Create prefabs for SFX and spawn them for each SFX.
    /// </summary>
    public class SoundManager : MonoBehaviour
    {
        /// <summary>
        /// Static instance of SoundManager which allows it to be accessed by any other script.
        /// </summary>
        public static SoundManager Instance = null;

        /// <summary>
        /// PREFAB of SFX handler object.
        /// </summary>
        public GameObject SfxHandlerPrefab;
        
        /// <summary>
        /// Drag a reference to the audio source which will play the music.
        /// </summary>
        public AudioSource BgMusicSource;
        
        /// <summary>
        /// The lowest a sound effect will be randomly pitched.
        /// </summary>
        public float LowPitchRange = .98f;
        
        /// <summary>
        /// The highest a sound effect will be randomly pitched.
        /// </summary>
        public float HighPitchRange = 1.02f;
        
        /// <summary>
        /// Default pitch.
        /// </summary>
        public float DefaultPitchRange = 1f;
        
        /// <summary>
        /// Previous background music.
        /// </summary>
        private AudioClip _previousBgMusic;
        
        /// <summary>
        /// Previous background music playback time.
        /// </summary>
        private float _previousBgMusicPlaybackTime;

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
                // Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a SoundManager.
                Destroy(gameObject);
            }

            // Sets this to not be destroyed when reloading scene.
            DontDestroyOnLoad(gameObject);
        }

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        /// <summary>
        /// Used to play single sound clips. 
        /// </summary>
        /// <param name="clip">Audio clip to play.</param>
        public void PlaySingleSfx(AudioClip clip)
        {
            // Create an handler.
            SfxHandler handler = Instantiate(SfxHandlerPrefab, Vector3.zero, Quaternion.identity).GetComponent<SfxHandler>();

            handler.SfxSource.clip = clip;
            handler.SfxSource.pitch = DefaultPitchRange;
            handler.SfxSource.Play();
            
            // Destroy the handler after expiration.
            Destroy(handler.gameObject, clip.length);
        }

        /// <summary>
        /// RandomizeSfx chooses randomly between various audio clips and slightly changes their pitch.
        /// </summary>
        /// <param name="clips">Audio clip to play.</param>
        public void PlayRandomizeSfx(AudioClip[] clips)
        {
            // Generate a random number between 0 and the length of our array of clips passed in.
            int randomIndex = Random.Range(0, clips.Length);

            // Choose a random pitch to play back our clip at between our high and low pitch ranges.
            float randomPitch = Random.Range(LowPitchRange, HighPitchRange);

            // Create an handler.
            SfxHandler handler = Instantiate(SfxHandlerPrefab, Vector3.zero, Quaternion.identity).GetComponent<SfxHandler>();

            handler.SfxSource.clip = clips[randomIndex];
            handler.SfxSource.pitch = randomPitch;
            handler.SfxSource.Play();
            
            // Destroy the handler after expiration.
            Destroy(handler.gameObject, clips[randomIndex].length);
        }

        /// <summary>
        /// Play a new background music. 
        /// </summary>
        /// <param name="clip">Audio clip to play.</param>
        public void PlayNewBackgroundMusic(AudioClip clip)
        {
            _previousBgMusic = BgMusicSource.clip;
            _previousBgMusicPlaybackTime = BgMusicSource.time;
            
            BgMusicSource.clip = clip;
            BgMusicSource.time = 0f;
            
            // Play the clip.
            BgMusicSource.Play();
        }

        /// <summary>
        /// Play previous background music. 
        /// </summary>
        /// <param name="continueOnPlaybackTime">Should music continue playing from the time whre it ends?</param>
        public void PlayPreviousBackgroundMusic(bool continueOnPlaybackTime)
        {
            AudioClip n = BgMusicSource.clip;
            BgMusicSource.clip = _previousBgMusic;
            _previousBgMusic = n;

            BgMusicSource.time = continueOnPlaybackTime ? _previousBgMusicPlaybackTime : 0f;
            
            // Play the clip.
            BgMusicSource.Play();
        }
    }
}