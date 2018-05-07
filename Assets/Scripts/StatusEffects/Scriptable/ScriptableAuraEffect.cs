using Characters;
using UnityEngine;

namespace StatusEffects.Scriptable
{
	[CreateAssetMenu(menuName = "StatusEffect/AuraEffect")]
	public class ScriptableAuraEffect : ScriptableStatusEffect
	{
		/// <summary>
		/// Effect which is applied on target.
		/// </summary>
		[Header("Effect Speciality")] public GameObject AuraEffectPrefab;
        
		public override StatusEffect Initialize(Character target, Character caster)
		{
			return new AuraEffect(this, target, caster);
		}
	}
}
