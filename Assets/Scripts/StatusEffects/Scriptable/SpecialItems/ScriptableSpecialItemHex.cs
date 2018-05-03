using Characters;
using StatusEffects.SpecialItems;
using UnityEngine;

namespace StatusEffects.Scriptable.SpecialItems
{
	[CreateAssetMenu(menuName = "StatusEffect/SpecialItem/Hex")]
	public class ScriptableSpecialItemHex : ScriptableStatusEffect
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
		
		/// <summary>
		/// Sound on start.
		/// </summary>
		public AudioClip StartSfx;
		
		/// <summary>
		/// Sound on end.
		/// </summary>
		public AudioClip EndSfx;
		
		public override StatusEffect Initialize(Character target, Character caster)
		{
			return new SpecialItemHex(this, target, caster);
		}
	}
}
