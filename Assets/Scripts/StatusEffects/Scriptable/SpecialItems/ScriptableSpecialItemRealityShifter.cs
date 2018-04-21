using Characters;
using StatusEffects.SpecialItems;
using UnityEngine;

namespace StatusEffects.Scriptable.SpecialItems
{
	[CreateAssetMenu(menuName = "StatusEffect/SpecialItem/RealityShifter")]
	public class ScriptableSpecialItemRealityShifter : ScriptableStatusEffect
	{
		/// <summary>
		/// Status effect which will be applied as debuff.
		/// </summary>
		[Header("Effect Speciality")] public ScriptableSwapControls ApplicableStatusEffect;
        
		public override StatusEffect Initialize(Character target, Character caster)
		{
			return new SpecialItemRealityShifter(this, target, caster);
		}
	}
}