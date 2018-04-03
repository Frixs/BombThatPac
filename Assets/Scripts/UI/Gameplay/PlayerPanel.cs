using Managers;
using UnityEngine;

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
		
		// Use this for initialization
		void Start ()
		{
		}
	
		// Update is called once per frame
		void Update ()
		{
		}
	}
}
