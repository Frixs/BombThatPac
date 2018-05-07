using Characters;
using Managers;
using StatusEffects.Scriptable;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace StatusEffects
{
	public class AuraEffect : StatusEffect
	{
		private GameObject _effectReference = null;
		
		public AuraEffect(ScriptableStatusEffect data, Character target, Character caster) : base(data, target, caster)
		{
		}

		protected override void Delay()
		{
			throw new System.NotImplementedException();
		}

		protected override void Activate()
		{
			_effectReference = SpawnManager.Instance.SpawnFollowingAnimationLoop(((ScriptableAuraEffect) Data).AuraEffectPrefab, Target.gameObject, Vector3.zero, Quaternion.identity);
		}

		protected override void End()
		{
			SpawnManager.Instance.DespawnAnimation(_effectReference, 0);
		}

		protected override void Repeat()
		{
			throw new System.NotImplementedException();
		}
	}
}
