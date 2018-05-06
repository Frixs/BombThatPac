using Characters;
using StatusEffects.SpecialItems;
using UnityEngine;

namespace StatusEffects.Scriptable.SpecialItems
{
	[CreateAssetMenu(menuName = "StatusEffect/SpecialItem/BombReplicator")]
	public class ScriptableSpecialItemBombReplicator : ScriptableStatusEffect
	{
		/// <summary>
		/// How many more bombs can player plant.
		/// </summary>
		[Header("Effect Speciality")] public int AdditionalBombs;
		
		/// <summary>
		/// Visible particles which represents the buff.
		/// </summary>
		public GameObject BuffEffectPrefab;
		
		/// <summary>
		/// Sound on start.
		/// </summary>
		public AudioClip StartSfx;
		
		public override StatusEffect Initialize(Character target, Character caster)
		{
			return new SpecialItemBombReplicator(this, target, caster);
		}
	}
}
