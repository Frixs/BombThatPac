using Characters;
using Managers;
using StatusEffects.Scriptable;
using UnityEngine;

namespace StatusEffects
{
	public class SwapControls : StatusEffect
	{
		/// <summary>
		/// Reference to spawned animation gameobject to be able to delete it after.
		/// </summary>
		private GameObject _animationReference = null;
		
		public SwapControls(ScriptableStatusEffect data, Character target, Character caster) : base(data, target, caster)
		{
		}

		protected override void Activate()
		{
			// Swap direction.
			Target.HasSwappedDirection = true;
			
			// Spawn animation.
			_animationReference = SpawnManager.Instance.SpawnFollowingAnimationLoop(
				((ScriptableSwapControls) Data).AnimationPrefab, 
				Target.gameObject, 
				new Vector3(0.25f, 1.3f, 0f), 
				Quaternion.identity
			);
		}

		protected override void End()
		{
			// Swap back direction.
			Target.HasSwappedDirection = false;
			
			// Despawn animation.
			SpawnManager.Instance.DespawnAnimation(_animationReference);
		}

		protected override void Repeat()
		{
			throw new System.NotImplementedException();
		}
	}
}
