using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Managers
{
	public class InputManager : MonoBehaviour
	{
		/// <summary>
		/// Static instance of InputManager which allows it to be accessed by any other script.
		/// </summary>
		public static InputManager Instance = null;

		/// <summary>
		/// Dictionary with all hotkeys.
		/// </summary>
		public Dictionary<string, Dictionary<string, KeyCode>> Hotkeys;

		// On script enable.
		void OnEnable()
		{
			// Set up all hotkeys.
			Hotkeys = new Dictionary<string, Dictionary<string, KeyCode>>() {
				{
					"Player1", new Dictionary<string, KeyCode>() {
						{ "MoveUp", 		KeyCode.W },
						{ "MoveLeft",		KeyCode.A },
						{ "MoveDown",		KeyCode.S },
						{ "MoveRight",		KeyCode.D },
						{ "Action",			KeyCode.Space },
						{ "SpecialAction",	KeyCode.R },
						{ "CollectItem",	KeyCode.F },
					}
				},
				{
					"Player2", new Dictionary<string, KeyCode>() {
						{ "MoveUp", 		KeyCode.UpArrow },
						{ "MoveLeft",		KeyCode.LeftArrow },
						{ "MoveDown",		KeyCode.DownArrow },
						{ "MoveRight",		KeyCode.RightArrow },
						{ "Action",			KeyCode.Return },
						{ "SpecialAction",	KeyCode.RightShift },
						{ "CollectItem",	KeyCode.RightControl },
					}
				},
			};
		}

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
				// Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a InputManager.
				Destroy(gameObject);
			}

			// Sets this to not be destroyed when reloading scene.
			DontDestroyOnLoad(gameObject);
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
		/// Get KeyCode by the section and name of the hotkey.
		/// </summary>
		/// <param name="section">Secion name.</param>
		/// <param name="hotkeyName">Hotkey name.</param>
		/// <returns></returns>
		public KeyCode GetButtonKeyCode(string section, string hotkeyName)
		{
			if (!Hotkeys.ContainsKey(section) || !Hotkeys[section].ContainsKey(hotkeyName))
			{
				Debug.unityLogger.LogFormat(LogType.Error, "No KeyCode for hotkey: {0} from the section: {1}!", hotkeyName, section);
				return KeyCode.None;
			}

			return Hotkeys[section][hotkeyName];
		}
	}
}
