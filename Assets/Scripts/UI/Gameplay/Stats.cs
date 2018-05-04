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

		private int _totalFragCount;
		
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
			Text fragCounter = GetComponentInChildren<Text>();
			fragCounter.text = "Coins: " + count;
			
			if (count >= MapManager.Instance.TotalFragmentCount)
			{
				fragCounter.text = "Find a portal!";
				GetComponent<Image>().color = new Color(0f, 255f, 0f, 0.6f);
			}
			else
			{
				GetComponent<Image>().color = new Color(216f, 216f, 216f, 0.6f);
			}
		}
	}
}
