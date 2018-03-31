using Characters;
using StatusEffects.Scriptable;

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

		protected override void Repeat()
		{
			throw new System.NotImplementedException();
		}
	}
}
