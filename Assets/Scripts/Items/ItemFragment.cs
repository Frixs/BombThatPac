using UnityEngine;
using UnityEngine.UI;

namespace Items
{
	public class ItemFragment : MonoBehaviour
	{
		/// <summary>
		/// How many items this game object holds as a stack.
		/// </summary>
		[HideInInspector]
		private int _quantity = 1;
		public int Quantity
		{
			get { return _quantity; }
			set { _quantity = value; }
		}

		/// <summary>
		/// Reference to UI quantity panel.
		/// </summary>
		public RectTransform UiPanelQuantity;
		
		/// <summary>
		/// Reference to UI quantity holder text.
		/// </summary>
		public Text UiTextQuantity;

		// Use this for initialization
		void Start ()
		{
			if (Quantity == 1)
				UiPanelQuantity.gameObject.SetActive(false);
			
			UiTextQuantity.text = "" + Quantity;
		}
	
		// Update is called once per frame
		void Update ()
		{
		}
	}
}
