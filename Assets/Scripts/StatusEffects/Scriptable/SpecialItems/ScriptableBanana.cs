using Characters;
using StatusEffects.SpecialItems;
using UnityEngine;

namespace StatusEffects.Scriptable.SpecialItems
{
	[CreateAssetMenu(menuName = "StatusEffect/SpecialItem/Banana")]
	public class ScriptableBanana : ScriptableStatusEffect
	{
		/// <summary>
		/// Banana prefab.
		/// </summary>
		[Header("Effect Speciality")] public GameObject BananaPeelPrefab;
		
		public override StatusEffect Initialize(Character target, Character caster)
		{
			return new Banana(this, target, caster);
		}
	}
}
