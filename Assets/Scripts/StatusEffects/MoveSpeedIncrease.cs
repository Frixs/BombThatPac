using Characters;
using StatusEffects.Scriptable;
using UnityEngine;

namespace StatusEffects
{
	public class MoveSpeedIncrease : StatusEffect
	{
		public MoveSpeedIncrease(ScriptableStatusEffect data, Character target) : base(data, target)
		{
		}

		protected override void Activate()
		{
			Target.MoveSpeed += ((ScriptableMoveSpeedIncrease) Data).SpeedIncrease;
		}

		protected override void End()
		{
			Target.MoveSpeed -= ((ScriptableMoveSpeedIncrease) Data).SpeedIncrease;
		}
	}
}
