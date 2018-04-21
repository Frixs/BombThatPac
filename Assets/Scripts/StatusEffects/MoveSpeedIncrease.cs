using Characters;
using Managers;
using StatusEffects.Scriptable;
using UnityEngine;

namespace StatusEffects
{
	public class MoveSpeedIncrease : StatusEffect
	{
		/// <summary>
		/// Reference to spawned animation gameobject to be able to delete it after.
		/// </summary>
		private GameObject _animationReference = null;

		/// <summary>
		/// Speed change. To be able to revert it to back on the end of effect.
		/// </summary>
		private float _moveAnimationSpeedChange;
		
		public MoveSpeedIncrease(ScriptableStatusEffect data, Character target, Character caster) : base(data, target, caster)
		{
		}

		protected override void Activate() // TODO check if value is under zero. + end this efect if target dies. + animation speed too.
		{
			float speedPctChange = Target.MoveSpeed / 100f; // Get 1% of current move speed.
			
			// Change move speed.
			Target.MoveSpeed += ((ScriptableMoveSpeedIncrease) Data).SpeedIncrease;
			
			speedPctChange = (100f - (Target.MoveSpeed / speedPctChange)) * (-1); // get difference in percentage. How many the move speed has changed after buff/debuff in percentage.
			_moveAnimationSpeedChange = speedPctChange * 0.01f; // Get percetange to range 0 - 1 (0.01 == 1%).
			Target.MyAnimator.speed += _moveAnimationSpeedChange;

			if (((ScriptableMoveSpeedIncrease) Data).AnimationPrefab != null)
			{
				_animationReference = SpawnManager.Instance.SpawnFollowingAnimationLoop(((ScriptableMoveSpeedIncrease) Data).AnimationPrefab, Target.gameObject, Vector3.zero, Quaternion.identity);
			}
		}

		protected override void End()
		{
			// Change move speed.
			Target.MoveSpeed -= ((ScriptableMoveSpeedIncrease) Data).SpeedIncrease;
			
			// Change move animation speed.
			Target.MyAnimator.speed -= _moveAnimationSpeedChange;
			
			if (_animationReference != null)
			{
				SpawnManager.Instance.DespawnAnimation(_animationReference);
			}
		}

		protected override void Repeat()
		{
			throw new System.NotImplementedException();
		}
	}
}
