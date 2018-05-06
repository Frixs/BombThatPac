using System.Collections;
using Characters;
using Managers;
using NUnit.Framework;
using StatusEffects.Scriptable;
using StatusEffects.Scriptable.SpecialItems;
using UnityEngine;

namespace StatusEffects.SpecialItems
{
	public class SpecialItemSwap : StatusEffect
	{
		private GameObject _delaySfx = null;
		private GameObject _delayEffectTarget = null;
		
		public SpecialItemSwap(ScriptableStatusEffect data, Character target, Character caster) : base(data, target, caster)
		{
		}

		protected override void Delay()
		{
			// Spawn delay animation.
			_delayEffectTarget = SpawnManager.Instance.SpawnFollowingAnimationLoop(((ScriptableSpecialItemSwap) Data).DelayEffectPrefab, Target.gameObject, Vector3.zero, Quaternion.identity);
			
			// Play sound.
			_delaySfx = SoundManager.Instance.PlaySingleSfx(((ScriptableSpecialItemSwap) Data).DelaySfx);
		}

		protected override void Activate()
		{
			// Apply this effect to PacMan as DummyUnit to handle whole effect control.
			StatusEffectManager.Instance.ApplyStatusEffect(
				GameManager.Instance.PacMan,
				Target,
				((ScriptableSpecialItemSwap) Data).ApplicableStatusEffect
			);
		}

		protected override void End()
		{
			SoundManager.Instance.StopPlayingSfx(_delaySfx);
			SpawnManager.Instance.DespawnAnimation(_delayEffectTarget);
		}

		protected override void Repeat()
		{
			throw new System.NotImplementedException();
		}
	}
}
