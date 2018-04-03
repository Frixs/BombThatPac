using Characters;
using StatusEffects.SpecialItems;
using UnityEngine;

namespace StatusEffects.Scriptable.SpecialItems
{
	[CreateAssetMenu(menuName = "StatusEffect/SpecialItem/Hex")]
	public class ScriptableHex : ScriptableStatusEffect
	{
		/// <summary>
		/// Hex controller.
		/// </summary>
		[Header("Effect Speciality")] public RuntimeAnimatorController MouseController;
		
		/// <summary>
		/// Morph/Puff explosion effect animation prefab.
		/// </summary>
		public GameObject ExplosionPrefab;

		/// <summary>
		/// How many speed should the status effect gain.
		/// </summary>
		public float MoveSpeedIncrease;
		
		public override StatusEffect Initialize(Character target, Character caster)
		{
			return new Hex(this, target, caster);
		}
	}
}
