using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Special
{
	public class GhostHouseDoor : MonoBehaviour
	{
		[SerializeField] private RuntimeAnimatorController _openAnimationController;
		[SerializeField] private RuntimeAnimatorController _closeAnimationController;
		[SerializeField] private AudioClip _openSfx;
		[SerializeField] private AudioClip _closeSfx;
		
		/// <summary>
		/// Opne the door and ignore collision.
		/// </summary>
		/// <param name="collidersToIgnore">Ignore collision with this collider and the door.</param>
		public void OpenDoor(List<Collider2D> collidersToIgnore)
		{
			if (collidersToIgnore == null)
			{
				Debug.unityLogger.Log(LogType.Error, "There is missing parameter to be able to open the door!");
				return;	
			}
			
			// Ignore phys. collider.
			foreach (var c in collidersToIgnore)
				Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), c);
			
			// Run animation.
			GetComponent<Animator>().runtimeAnimatorController = _openAnimationController;
			
			// Play sound.
			SoundManager.Instance.PlaySingleSfx(_openSfx);
		}
		
		/// <summary>
		/// Close the door animation with reactivationg collisions.
		/// </summary>
		/// <param name="collidersToReactivate">Collider to reactivate collisions with the door.</param>
		public void CloseDoor(List<Collider2D> collidersToReactivate)
		{
			if (collidersToReactivate == null)
			{
				Debug.unityLogger.Log(LogType.Error, "There is missing parameter to be able to close the door!");
				return;	
			}
			
			// Reactivate phys. collider.
			foreach (var c in collidersToReactivate)
				Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), c, false);
			
			// Run animation.
			GetComponent<Animator>().runtimeAnimatorController = _closeAnimationController;
			
			// Play sound.
			SoundManager.Instance.PlaySingleSfx(_closeSfx);
		}
	}
}
