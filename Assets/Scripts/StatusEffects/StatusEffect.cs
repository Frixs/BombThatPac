using System;
using Characters;
using StatusEffects.Scriptable;
using UnityEngine;

namespace StatusEffects
{
	public abstract class StatusEffect
	{
		/// <summary>
		/// Status effect duration timer.
		/// </summary>
		private float _durationTimer;
		
		/// <summary>
		/// Start delay timer to activate status effect.
		/// </summary>
		private float _startDelayTimer;
		
		/// <summary>
		/// Damage over time timer.
		/// </summary>
		private float _repeatTimer;

		/// <summary>
		/// Get information about the status effect if has been already activated.
		/// </summary>
		private bool _isAlreadyActivated;
		
		/// <summary>
		/// Reference to Scriptable status effects which storing data about the status effect.
		/// </summary>
		public ScriptableStatusEffect Data { get; protected set; }
		
		/// <summary>
		/// Target which has applied this status effect.
		/// </summary>
		public readonly Character Target;
		
		/// <summary>
		/// Caster which casted the status effect.
		/// </summary>
		public readonly Character Caster;
		
		/// <summary>
		/// Check if status effect already expired.
		/// </summary>
		public bool IsFinished => !Data.IsPermanent && _durationTimer <= 0f;

		/// <summary>
		/// Initialize the status effect via constructor.
		/// </summary>
		/// <param name="data">Reference to Scriptable status effects which storing data about the status effect.</param>
		/// <param name="target">Target which has applied this status effect.</param>
		/// <param name="caster">Caster which casted the status effect.</param>
		protected StatusEffect(ScriptableStatusEffect data, Character target, Character caster)
		{
			Data = data;
			Target = target;
			Caster = caster;

			_durationTimer = Data.Duration;
			_startDelayTimer = Data.StartActivationDelay;
			_repeatTimer = Data.RepeatTime;
			_isAlreadyActivated = false;
		}

		/// <summary>
		/// Timer processes.
		/// </summary>
		/// <param name="delta">Delta time of the game.</param>
		public void Tick(float delta)
		{
			// Start delay if some exists.
			if (_startDelayTimer > 0f)
			{
				_startDelayTimer -= delta;
				return;
			}
			
			// Activate the status effect.
			if (!_isAlreadyActivated)
			{
				Activate();
				_isAlreadyActivated = true;
			}

			// Check if status effect is timed (or permanent).
			if (!Data.IsPermanent)
			{
				// Main duration timer.
				_durationTimer -= delta;
				if (_durationTimer <= 0f)
				{
					End();
					Debug.unityLogger.LogFormat(LogType.Log, "[{0}] Status effect ({1}) ended!", Target.Name, this);
					return;
				}
			}

			// Repeating process.
			if (Data.RepeatTime > 0f)
			{
				_repeatTimer -= delta;
				if (_repeatTimer <= 0f)
				{
					_repeatTimer = Data.RepeatTime;
					Repeat();
				}
			}
		}

		/// <summary>
		/// Activate and start the status effect.
		/// </summary>
		protected abstract void Activate();
		
		/// <summary>
		/// End the status effect.
		/// </summary>
		protected abstract void End();
		
		/// <summary>
		/// Repeat some procedure each each tick according to repeat timer.
		/// </summary>
		protected abstract void Repeat();

		/// <summary>
		/// Force End the status effect manually.
		/// </summary>
		public void ForceEnd()
		{
			End();
		}
		
		/// <summary>
		/// Enum type of status effect.
		/// </summary>
		public enum StatusEffectType
		{
			Buff,
			Debuff,
			ItemActivator
		}
	}
}
