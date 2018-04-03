using System.Linq;
using Characters;
using Items.SpecialItems;
using StatusEffects.Scriptable;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay
{
	public class Inventory : MonoBehaviour
	{
		/// <summary>
		/// Reference to player panel.
		/// </summary>
		public PlayerPanel PlayerPanelReference;
		
		// Use this for initialization
		void Start ()
		{
		}
	
		// Update is called once per frame
		void Update ()
		{
		}

		/// <summary>
		/// Add item to player and its inventory.
		/// </summary>
		/// <param name="target">Target Player character which will get the item.</param>
		/// <param name="item">Item to add.</param>
		public void AddItem(Player target, SpecialItem item)
		{
			Sprite previousItem = null;

			// Go through all item slots.
			for (int i = 0; i < transform.childCount; i++)
			{
				// Get image component of the slot.
				Image img = transform.GetChild(i).GetChild(0).GetComponent<Image>();

				// First slot.
				if (i == 0)
				{
					img.enabled = true;
                        
					previousItem = img.sprite;
					img.sprite = item.GetComponent<SpriteRenderer>().sprite;
					continue;
				}
				
				// Another slots.
				// If there is no item to move from the first slot, we dont need to continue.
				if (previousItem == null)
					break;

				img.enabled = true;
                
				Sprite currentItem = img.sprite; // Auxiliary variable.
				img.sprite = previousItem;
				previousItem = currentItem;

				// Remove item which overflows.
				if (i == transform.childCount - 1 && previousItem != null)
					target.SpecialItemList.Remove(target.SpecialItemList.First());
			}
            
			// Add item's special status effect to player's inventory.
			target.SpecialItemList.Add(item.GetComponent<SpecialItem>().ItemStatusEffect);
		}

		/// <summary>
		/// Removes the first item in the inventory. (Programically it is the last item in the List).
		/// </summary>
		/// <param name="target">Target player character of interest.</param>
		public void RemoveItemAtFirstPos(Player target)
		{
			Sprite previousItem = null;

			if (target.SpecialItemList.Count == 0)
				return;
			
			for (int i = transform.childCount - 1; i >= 0; i--)
			{
				// Get image component of the slot.
				Image img = transform.GetChild(i).GetChild(0).GetComponent<Image>();

				if (!img.enabled)
					continue;
				
				Sprite currentItem = img.sprite; // Auxiliary variable.
				img.sprite = previousItem;
				previousItem = currentItem;
				
				if (img.sprite == null)
					img.enabled = false;
			}
			
			// Remove the first item in the inventory.
			target.SpecialItemList.Remove(target.SpecialItemList.Last());
		}
	}
}
