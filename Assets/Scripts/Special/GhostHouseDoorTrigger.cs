using Characters;
using UnityEngine;

namespace Special
{
	public class GhostHouseDoorTrigger : MonoBehaviour
	{
		private void OnTriggerEnter2D(Collider2D other)
		{
			Ghost obj = null;

			if ((obj = other.GetComponent<Ghost>()) != null)
			{
				obj.IsInGhostHouse = false;
			}
		}
	}
}
