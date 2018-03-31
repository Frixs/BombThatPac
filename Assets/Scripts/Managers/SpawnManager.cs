using System.Collections;
using Characters;
using UnityEngine;

namespace Managers
{
	/// <summary>
	/// This class controlls all possible spawn on the map.
	/// </summary>
	public class SpawnManager : MonoBehaviour
	{
		/// <summary>
		/// Static instance of SpawnManager which allows it to be accessed by any other script.
		/// </summary>
		public static SpawnManager Instance = null;
		
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
				// Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a SpawnManager.
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
		/// Respawn the character.
		/// </summary>
		/// <param name="toRespawn">GameObject reference to respawn.</param>
		/// <param name="delay">Time to respawn.</param>
		/// <param name="positions">Spawnpoint positions, whre the object can possibly spawn.</param>
		public void RespawnCharacterInit(Character toRespawn, float delay, Transform[] positions)
		{
			if (toRespawn == null || positions.Length == 0)
			{
				Debug.unityLogger.Log(LogType.Error, "There is missing parameter to be able to respawn!");
				return;
			}

			StartCoroutine(RespawnCharacter(toRespawn, delay, positions));
		}

		/// <summary>
		/// Coroutine delayed respawn object.
		/// </summary>
		/// <param name="toRespawn">GameObject reference to respawn.</param>
		/// <param name="delay">Time to respawn.</param>
		/// <param name="positions">Spawnpoint positions, whre the object can possibly spawn.</param>
		/// <returns></returns>
		private IEnumerator RespawnCharacter(Character toRespawn, float delay, Transform[] positions)
		{
			yield return new WaitForSeconds(delay);

			toRespawn.transform.position = positions[Random.Range(0, positions.Length)].position;
			toRespawn.gameObject.SetActive(true);
			Debug.unityLogger.LogFormat(LogType.Log, "[{0} ({1})] Character has been respawned!", toRespawn.GetComponent<Character>().Identifier, toRespawn.GetComponent<Character>().Name);
		    
			toRespawn.GetComponent<Character>().IsDeath = false;

			// If it is Player, set invulnerability on respawn.
			if (toRespawn is Player)
				StatusEffectManager.Instance.ApplyStatusEffect(toRespawn, ((Player) toRespawn).RespawnInvulStatusEffect.Initialize(toRespawn));
		}
	}
}
