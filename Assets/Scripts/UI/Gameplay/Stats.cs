using Items;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay
{
	public class Stats : MonoBehaviour
	{
		/// <summary>
		/// Reference to player panel.
		/// </summary>
		public PlayerPanel PlayerPanelReference;

		private Color _defaultScoreLabelColor = Color.clear;
		
		// Use this for initialization
		void Start ()
		{
		}
	
		// Update is called once per frame
		void Update ()
		{
		}

		/// <summary>
		/// Update data count of player's fragment in player panel.
		/// </summary>
		/// <param name="count">Number to update.</param>
		public void UpdateFragmentCount(int count)
		{
			if (_defaultScoreLabelColor == Color.clear)
				_defaultScoreLabelColor = GetComponent<Image>().color;
			
			Text fragCounter = GetComponentInChildren<Text>();
			fragCounter.text = "" + count;
			
			if (count >= MapManager.Instance.TotalFragmentCount)
			{
				fragCounter.text = "Find a portal!";
				GetComponent<Image>().color = new Color(0f, 255f, 0f, 1f);
			}
			else
			{
				GetComponent<Image>().color = _defaultScoreLabelColor;
			}
		}
	}
}
