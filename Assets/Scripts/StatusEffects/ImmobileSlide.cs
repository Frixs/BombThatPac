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

		/// <summary>
		/// Reference to be able to stop SFX of stun.
		/// </summary>
		private GameObject _sfxToDespawnInTheEnd;
		
		public ImmobileSlide(ScriptableStatusEffect data, Character target, Character caster) : base(data, target, caster)
		{
		}

		protected override void Delay()
		{
			throw new System.NotImplementedException();
		}

		protected override void Activate()
		{
			Target.DisableActions();
			Target.MyRigidBody.AddForce(Target.transform.forward.normalized * ((ScriptableImmobileSlide) Data).SlideSpeed);
			
			_animationToDespawnInTheEnd = SpawnManager.Instance.SpawnFollowingAnimationLoop(((ScriptableImmobileSlide) Data).StunAnimationPrefab, Target.gameObject, Vector3.zero, Quaternion.identity);
			
			// Play sound.
			_sfxToDespawnInTheEnd = SoundManager.Instance.PlaySingleSfx(((ScriptableImmobileSlide) Data).StartSfx);
		}

		protected override void End()
		{
			SpawnManager.Instance.DespawnAnimation(_animationToDespawnInTheEnd);
			
			// Stop playing the SFX.
			SoundManager.Instance.StopPlayingSfx(_sfxToDespawnInTheEnd);
			
			Target.EnableActions();
		}

		protected override void Repeat()
		{
		}
	}
}
