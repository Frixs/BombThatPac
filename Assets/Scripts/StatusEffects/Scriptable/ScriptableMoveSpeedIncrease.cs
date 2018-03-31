using Characters;
using UnityEngine;

namespace StatusEffects.Scriptable
{
	[CreateAssetMenu(menuName = "StatusEffects/MoveSpeedIncrease")]
	public class ScriptableMoveSpeedIncrease : ScriptableStatusEffect
	{
		/// <summary>
		/// Speed to be increased (or deecreased).
		/// </summary>
		[Header("Effect Speciality")] public float SpeedIncrease;

		public override StatusEffect Initialize(Character obj)
		{
			return new MoveSpeedIncrease(this, obj);
		}
	}
}
