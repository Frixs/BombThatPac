using System;
using System.Collections.Generic;
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
		public Dictionary<string, Dictionary<InputButtonType, string>> Hotkeys;

		// On script enable.
		void OnEnable()
		{
			// Set up all hotkeys.
			Hotkeys = new Dictionary<string, Dictionary<InputButtonType, string>>() {
				{
					"Player1", new Dictionary<InputButtonType, string>() {
						{ InputButtonType.MoveUp, 		InputNameHandler.MoveUp },
						{ InputButtonType.MoveLeft,		InputNameHandler.MoveLeft },
						{ InputButtonType.MoveDown,		InputNameHandler.MoveDown },
						{ InputButtonType.MoveRight,	InputNameHandler.MoveRight },
						{ InputButtonType.Bomb,			InputNameHandler.Bomb },
						{ InputButtonType.UseItem,		InputNameHandler.UseItem },
					}
				},
				{
					"Player2", new Dictionary<InputButtonType, string>() {
						{ InputButtonType.MoveUp, 		InputNameHandler.MoveUpP2 },
						{ InputButtonType.MoveLeft,		InputNameHandler.MoveLeftP2 },
						{ InputButtonType.MoveDown,		InputNameHandler.MoveDownP2 },
						{ InputButtonType.MoveRight,	InputNameHandler.MoveRightP2 },
						{ InputButtonType.Bomb,			InputNameHandler.BombP2 },
						{ InputButtonType.UseItem,		InputNameHandler.UseItemP2 },
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
		/// <param name="hotkeyName">Hotkey enum.</param>
		/// <returns></returns>
		public string GetButton(string section, InputButtonType hotkeyName)
		{
			if (!Hotkeys.ContainsKey(section) || !Hotkeys[section].ContainsKey(hotkeyName))
			{
				Debug.unityLogger.LogFormat(LogType.Error, "No KeyCode for hotkey: {0} from the section: {1}!", hotkeyName, section);
				return string.Empty;
			}

			return Hotkeys[section][hotkeyName];
		}
	}

	/// <summary>
	/// This class handle all string references to input set in Unity input manager.
	/// </summary>
	public static class InputNameHandler
	{
		// Player 1.
		public const string MoveUp = "Move Up";
		public const string MoveDown = "Move Down";
		public const string MoveLeft = "Move Left";
		public const string MoveRight = "Move Right";
		public const string Bomb = "Bomb";
		public const string UseItem = "Use Item";
		
		// Player 2.
		public const string MoveUpP2 = "Move Up (Player 2)";
		public const string MoveDownP2 = "Move Down (Player 2)";
		public const string MoveLeftP2 = "Move Left (Player 2)";
		public const string MoveRightP2 = "Move Right (Player 2)";
		public const string BombP2 = "Bomb (Player 2)";
		public const string UseItemP2 = "Use Item (Player 2)";
	}

	/// <summary>
	/// Type of the buttons.
	/// </summary>
	public enum InputButtonType
	{
		MoveUp,
		MoveDown,
		MoveLeft,
		MoveRight,
		Bomb,
		UseItem,
	}
}
