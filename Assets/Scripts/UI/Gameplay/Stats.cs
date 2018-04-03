using Items;
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
		/// 
		/// </summary>
		/// <param name="count"></param>
		public void UpdateFragmentCount(int count)
		{
			Text fragCounter = GetComponentInChildren<Text>();
			fragCounter.text = "Fragments: " + count;
			
			// TODO: Prototype speciality only!
			if (_totalFragCount == 0)
				_totalFragCount = GameObject.Find("Fragments").GetComponentsInChildren<Fragment>().Length;
			
			if (count >= _totalFragCount)
			{
				fragCounter.text = "WINNER!";
				GetComponent<Image>().color = new Color(0f, 255f, 0f, 0.6f);
			}
			else
			{
				GetComponent<Image>().color = new Color(216f, 216f, 216f, 0.6f);
			}
		}
	}
}
