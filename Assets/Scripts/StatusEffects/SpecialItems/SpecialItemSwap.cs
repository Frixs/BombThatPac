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
		public SpecialItemSwap(ScriptableStatusEffect data, Character target, Character caster) : base(data, target, caster)
		{
		}

		protected override void Delay()
		{
			throw new System.NotImplementedException();
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
		}

		protected override void Repeat()
		{
			throw new System.NotImplementedException();
		}
	}
}
