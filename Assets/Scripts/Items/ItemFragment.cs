using Managers;
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

		/// <summary>
		/// Sprite to change if the amount is very large.
		/// </summary>
		[SerializeField] private Sprite _bagSprite;
		
		/// <summary>
		/// Particles for the bag sprite.
		/// </summary>
		[SerializeField] private GameObject _bagParticles;

		// Use this for initialization
		void Start ()
		{
			if (Quantity == 1)
				UiPanelQuantity.gameObject.SetActive(false);
			
			UiTextQuantity.text = "" + Quantity;

			// If the amount of coins is larger than X%, change appearance of the fragment.
			if (Quantity > MapManager.Instance.TotalFragmentCount * .7f) // 70% and more
			{
				gameObject.GetComponent<SpriteRenderer>().sprite = _bagSprite;
				gameObject.GetComponent<Animator>().enabled = false;
				// Add particles.
				SpawnManager.Instance.SpawnFollowingAnimationLoop(_bagParticles, gameObject, Vector3.zero, Quaternion.identity);
			}
		}
	
		// Update is called once per frame
		void Update ()
		{
		}
	}
}
