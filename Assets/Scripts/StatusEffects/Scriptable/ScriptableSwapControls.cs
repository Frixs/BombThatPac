using Characters;
using UnityEngine;

namespace StatusEffects.Scriptable
{
	[CreateAssetMenu(menuName = "StatusEffect/SwapControls")]
	public class ScriptableSwapControls : ScriptableStatusEffect
	{
		/// <summary>
		/// Animation PREFAB.
		/// </summary>
		[Header("Effect Speciality")] public GameObject AnimationPrefab;

		public override StatusEffect Initialize(Character target, Character caster)
		{
			return new SwapControls(this, target, caster);
		}
	}
}
