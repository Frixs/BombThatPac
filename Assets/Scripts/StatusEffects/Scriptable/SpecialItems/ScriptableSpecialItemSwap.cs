using Characters;
using StatusEffects.SpecialItems;
using UnityEngine;

namespace StatusEffects.Scriptable.SpecialItems
{
	[CreateAssetMenu(menuName = "StatusEffect/SpecialItem/Swap")]
	public class ScriptableSpecialItemSwap : ScriptableStatusEffect
	{
		/// <summary>
		/// Status effect which will be applied as buff.
		/// </summary>
		[Header("Effect Speciality")] public ScriptableSwapPositions ApplicableStatusEffect;
		
		public override StatusEffect Initialize(Character target, Character caster)
		{
			return new SpecialItemSwap(this, target, caster);
		}
	}
}
