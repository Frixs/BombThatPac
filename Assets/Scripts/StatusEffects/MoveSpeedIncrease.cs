using Characters;
using StatusEffects.Scriptable;

namespace StatusEffects
{
	public class MoveSpeedIncrease : StatusEffect
	{
		public MoveSpeedIncrease(ScriptableStatusEffect data, Character target, Character caster) : base(data, target, caster)
		{
		}

		protected override void Activate() // TODO check if value is under zero. + end this efect if target dies.
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
