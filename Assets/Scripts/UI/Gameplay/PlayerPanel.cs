using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay
{
	public class PlayerPanel : MonoBehaviour
	{
		/// <summary>
		/// Reference to the player.
		/// </summary>
		[HideInInspector] public PlayerManager PlayerManagerReference;
		
		/// <summary>
		/// Reference to player inventory.
		/// </summary>
		public Inventory PlayerInventory;
		
		/// <summary>
		/// Reference to player stats view.
		/// </summary>
		public Stats PlayerStats;
		
		/// <summary>
		/// Reference to death placeholder.
		/// </summary>
		public DeathPlaceholder DeathPlaceholder;

		/// <summary>
		/// Reference to object which handles inventory bag background.
		/// </summary>
		public GameObject BagBackground;

		/// <summary>
		/// Bag sprite of idle.
		/// </summary>
		public Sprite BagIdleSprite;
		
		/// <summary>
		/// Bag sprite of death.
		/// </summary>
		public Sprite BagDeathSprite;
		
		// Use this for initialization
		void Start ()
		{
		}
	
		// Update is called once per frame
		void Update ()
		{
			ManageDeathPlaceholder();
		}

		/// <summary>
		/// Switch on/off player death label.
		/// </summary>
		private void ManageDeathPlaceholder()
		{
			//DeathPlaceholder.gameObject.SetActive(PlayerManagerReference.PlayerComponent.IsDeath);

			if (PlayerManagerReference.PlayerComponent.IsDeath)
			{
				if (BagBackground.GetComponent<Image>().sprite != BagDeathSprite)
					BagBackground.GetComponent<Image>().sprite = BagDeathSprite;
			}
			else
			{
				if (BagBackground.GetComponent<Image>().sprite != BagIdleSprite)
					BagBackground.GetComponent<Image>().sprite = BagIdleSprite;
			}
		}
	}
}
