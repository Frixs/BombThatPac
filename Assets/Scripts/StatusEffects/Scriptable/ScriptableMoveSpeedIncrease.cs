using Characters;
using UnityEngine;

namespace StatusEffects.Scriptable
{
	[CreateAssetMenu(menuName = "StatusEffect/MoveSpeedIncrease")]
	public class ScriptableMoveSpeedIncrease : ScriptableStatusEffect
	{
		/// <summary>
		/// Speed to be increased (or deecreased).
		/// </summary>
		[Header("Effect Speciality")] public float SpeedIncrease;

		/// <summary>
		/// Animation PREFAB. It can be NULL if you do not want to animate anything.
		/// </summary>
		public GameObject AnimationPrefab;

		public override StatusEffect Initialize(Character target, Character caster)
		{
			return new MoveSpeedIncrease(this, target, caster);
		}
	}
}
