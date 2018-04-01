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
		public void ApplyStatusEffect(Character target, Character caster, ScriptableStatusEffect newScriptableStatusEffect)
		{
			if (target == null || newScriptableStatusEffect == null)
			{
				Debug.unityLogger.Log(LogType.Error, "Null reference for adding a new status effect!");
				return;
			}

			// Get status effect.
			StatusEffect newStatusEffect = newScriptableStatusEffect.Initialize(target, caster);
			
			bool effectOccurrence = target.AppliedStatusEffects.Exists(item => item.Data == newScriptableStatusEffect);
			
			// Don't let it create a new status effect of the same type if the status effect is not stackable and there is already one in.
			if (!newStatusEffect.Data.IsStackable && effectOccurrence)
				return;
			
			// Do not apply a new status effect if the status effect cannot be overwritten and this type of effect is already in.
			if (newStatusEffect.Data.CanBeOverwritten && effectOccurrence)
				target.AppliedStatusEffects.RemoveAll(item => item.Data == newScriptableStatusEffect);
			else if (!newStatusEffect.Data.CanBeOverwritten && effectOccurrence)
				return;

			target.AppliedStatusEffects.Add(newStatusEffect);
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
		/// Manually removes selected status effect/s.
		/// </summary>
		/// <param name="target">Target who will get a new status effect.</param>
		/// <param name="caster">Caster who applied status effect on the target. Can be NULL.</param>
		/// <param name="newScriptableStatusEffect">New scriptable status effect to be applied.</param>
		/// <param name="isCasterImportant">Let's say, if the method should care about caster or caster is caster should not be included to filter status effect to remove.</param>
		/// <param name="shouldRemoveAllOfTheSameType">Should the method remove all the status effects of the same type (only from the same caster if it it set).</param>
		public void RemoveStatusEffect(Character target, Character caster, ScriptableStatusEffect newScriptableStatusEffect, bool isCasterImportant, bool shouldRemoveAllOfTheSameType)
		{
			// Remove all effects of the same type.
			if (shouldRemoveAllOfTheSameType)
			{
				target.AppliedStatusEffects.RemoveAll(delegate(StatusEffect item)
				{
					if (isCasterImportant)
						return item.Data == newScriptableStatusEffect && item.Caster == caster;
					
					return item.Data == newScriptableStatusEffect;
				});
			}
			// Remove only the first occured status effect.
			else
			{
				target.AppliedStatusEffects.Remove(target.AppliedStatusEffects.First(delegate(StatusEffect item)
				{
					if (isCasterImportant)
						return item.Data == newScriptableStatusEffect && item.Caster == caster;
					
					return item.Data == newScriptableStatusEffect;
				}));
			}
		}
	}
}
