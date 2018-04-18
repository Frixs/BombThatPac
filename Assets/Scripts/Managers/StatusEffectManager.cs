using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Characters;
using StatusEffects;
using StatusEffects.Scriptable;
using UnityEngine;

namespace Managers
{
	public class StatusEffectManager : MonoBehaviour
	{
		/// <summary>
		/// Static instance of StatusEffectManager which allows it to be accessed by any other script.
		/// </summary>
		public static StatusEffectManager Instance = null;
		
		// Awake is always called before any Start functions
		void Awake()
		{
			// Check if instance already exists.
			if (Instance == null)
			{
				// If not, set instance to this.
				Instance = this;
			}
			// If instance already exists and it's not this.
			else if (Instance != this)
			{
				// Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a StatusEffectManager.
				Destroy(gameObject);
			}

			// Sets this to not be destroyed when reloading scene.
			//DontDestroyOnLoad(gameObject);
		}

		/// <summary>
		/// Apply a new status effect to a list of other effects which are already applied.
		/// </summary>
		/// <param name="target">Target who will get a new status effect.</param>
		/// <param name="caster">Caster who applied status effect on the target. Can be NULL.</param>
		/// <param name="newScriptableStatusEffect">New scriptable status effect to be applied.</param>
		/// <returns>TRUE: Status effect has been applied!</returns>
		public bool ApplyStatusEffect(Character target, Character caster, ScriptableStatusEffect newScriptableStatusEffect)
		{
			if (target == null || newScriptableStatusEffect == null)
			{
				Debug.unityLogger.Log(LogType.Error, "Null reference for adding a new status effect!");
				return false;
			}

			if (target.IsStatusEffectImmune)
				return false;

			// Get status effect.
			StatusEffect newStatusEffect = newScriptableStatusEffect.Initialize(target, caster);
			
			bool typeOccurrence = target.AppliedStatusEffects.Exists(item => item.Data.GetType() == newScriptableStatusEffect.GetType());
			bool effectOccurrence = target.AppliedStatusEffects.Exists(item => item.Data == newScriptableStatusEffect);

			// Overwrite the status effect if there are some of the same type already in.
			if (newStatusEffect.Data.OverwriteTheSameTypes && typeOccurrence)
				RemoveStatusEffect(target, caster, newScriptableStatusEffect, false, RemoveMethod.RemoveAllOfTheSameType);

			// Overwrite the status effect if there are some of the same effect already in.
			else if (newStatusEffect.Data.OverwriteTheSameEffects && effectOccurrence)
				RemoveStatusEffect(target, caster, newScriptableStatusEffect, false, RemoveMethod.RemoveAllOfTheSameEffect);
			
			// Don't let it create a new status effect of the same effect if the status effect is not stackable and there is already one in.
			else if (!newStatusEffect.Data.IsStackable && effectOccurrence)
				return false;

			Debug.unityLogger.LogFormat(LogType.Log, "[{0}] Status effect ({1}) applied!", target.Name, newStatusEffect);
			target.AppliedStatusEffects.Add(newStatusEffect);

			return true;
		}
		
		/// <summary>
		/// Process all character's status effects.
		/// </summary>
		/// <param name="target">Target to process its status effects.</param>
		public void ProcessStatusEffects(Character target)
		{
			if (target == null)
			{
				Debug.unityLogger.Log(LogType.Error, "Null reference for processing status effects!");
				return;
			}
			
			foreach (StatusEffect statusEffect in target.AppliedStatusEffects.ToArray())
			{
				statusEffect.Tick(Time.deltaTime);
				if (statusEffect.IsFinished)
				{
					target.AppliedStatusEffects.Remove(statusEffect);
				}
			}
		}

		/// <summary>
		/// Remove all status effects which have to be removed at death.
		/// </summary>
		/// <param name="target">Target to remove its status effects.</param>
		public void RemoveRequiredAtDeath(Character target)
		{
			if (target == null)
			{
				Debug.unityLogger.Log(LogType.Error, "Null reference!");
				return;
			}
			
			foreach (StatusEffect statusEffect in target.AppliedStatusEffects.ToArray())
			{
				if (statusEffect.Data.RemoveAtDeath)
				{
					statusEffect.ForceEnd();
					target.AppliedStatusEffects.Remove(statusEffect);
				}
			}
		}

		/// <summary>
		/// Manually removes selected status effect/s.
		/// </summary>
		/// <param name="target">Target who will get a new status effect.</param>
		/// <param name="caster">Caster who applied status effect on the target. Can be NULL.</param>
		/// <param name="scriptableStatusEffect">New scriptable status effect to be applied.</param>
		/// <param name="isCasterImportant">Should the caster be added to filter for removation method?</param>
		/// <param name="method">Method of the removation.</param>
		public void RemoveStatusEffect(Character target, Character caster, ScriptableStatusEffect scriptableStatusEffect, bool isCasterImportant, RemoveMethod method)
		{
			if (target == null || scriptableStatusEffect == null)
			{
				Debug.unityLogger.Log(LogType.Error, "Null reference for removing a status effect!");
				return;
			}

			// Remove all status effects of the same type.
			if (method == RemoveMethod.RemoveAllOfTheSameType)
			{
				target.AppliedStatusEffects.RemoveAll(delegate(StatusEffect item)
				{
					if (item.Data.GetType() != scriptableStatusEffect.GetType())
						return false;

					if (isCasterImportant)
						if (item.Caster != caster)
							return false;
					
					item.ForceEnd();
					Debug.unityLogger.LogFormat(LogType.Log, "[{0}] Status effect ({1}) removed!", target.Name, item);
					return true;
				});
			}
			// Remove all effects of the same effect.
			else if (method == RemoveMethod.RemoveAllOfTheSameEffect)
			{
				target.AppliedStatusEffects.RemoveAll(delegate(StatusEffect item)
				{
					if (item.Data != scriptableStatusEffect)
						return false;

					if (isCasterImportant)
						if (item.Caster != caster)
							return false;
					
					item.ForceEnd();
					Debug.unityLogger.LogFormat(LogType.Log, "[{0}] Status effect ({1}) removed!", target.Name, item);
					return true;
				});
			}
			// Remove only the first occured status effect.
			else if (method == RemoveMethod.RemoveTheFirst)
			{
				target.AppliedStatusEffects.Remove(target.AppliedStatusEffects.First(delegate(StatusEffect item)
				{
					if (item.Data != scriptableStatusEffect)
						return false;

					if (isCasterImportant)
						if (item.Caster != caster)
							return false;
					
					item.ForceEnd();
					Debug.unityLogger.LogFormat(LogType.Log, "[{0}] Status effect ({1}) removed!", target.Name, item);
					return true;
				}));
			}
		}
		
		/// <summary>
		/// Removation methods for RemoveStatusEffect() method.
		/// </summary>
		public enum RemoveMethod
		{
			/// <summary>
			/// Should the method remove all the status effects of the same type (like MoveSpeedIncrease)?
			/// </summary>
			RemoveAllOfTheSameType,
			
			/// <summary>
			/// Should the method remove all the status effects of the same effect (scriptable form)?
			/// </summary>
			RemoveAllOfTheSameEffect,
			
			/// <summary>
			/// Remove the first occured status effect.
			/// </summary>
			RemoveTheFirst,
		}
	}
}
