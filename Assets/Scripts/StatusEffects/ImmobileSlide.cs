using Characters;
using Managers;
using StatusEffects.Scriptable;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace StatusEffects
{
	public class ImmobileSlide : StatusEffect
	{
		/// <summary>
		/// Reference to be able to despawn animation of stun.
		/// </summary>
		private GameObject _animationToDespawnInTheEnd;
		
		public ImmobileSlide(ScriptableStatusEffect data, Character target, Character caster) : base(data, target, caster)
		{
		}

		protected override void Activate()
		{
			Target.DisableActions();
			Target.MyRigidBody.AddForce(Target.transform.forward.normalized * ((ScriptableImmobileSlide) Data).SlideSpeed);
			
			_animationToDespawnInTheEnd = SpawnManager.Instance.SpawnFollowingAnimationLoop(((ScriptableImmobileSlide) Data).StunAnimationPrefab, Target.gameObject, Vector3.zero, Quaternion.identity);
		}

		protected override void End()
		{
			SpawnManager.Instance.DespawnAnimation(_animationToDespawnInTheEnd);
			
			Target.EnableActions();
		}

		protected override void Repeat()
		{
		}
	}
}
