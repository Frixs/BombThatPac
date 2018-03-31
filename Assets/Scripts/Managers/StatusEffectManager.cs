using System.Collections.Generic;
using System.Linq;
using Characters;
using StatusEffects;
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
		/// <param name="newStatusEffect">New status effect to be applied.</param>
		public void ApplyStatusEffect(Character target, StatusEffect newStatusEffect) // TODO Try to look at using this method. There have to be 2 definition of the same target in 2 parameters. It is wierd.
		{
			bool effectOccurrence = target.AppliedStatusEffects.Any(item => item.GetType() == newStatusEffect.GetType());
			
			// Don't let it create a new status effect of the same type if the status effect is not stackable and there is already one in.
			if (!newStatusEffect.Data.IsStackable && effectOccurrence)
				return;
			
			// Do not apply a new status effect if the status effect cannot be overwritten and this type of effect is already in.
			if (newStatusEffect.Data.CanBeOverwritten && effectOccurrence)
				target.AppliedStatusEffects.RemoveAll(item => item.GetType() == newStatusEffect.GetType());
			else if (!newStatusEffect.Data.CanBeOverwritten && effectOccurrence)
				return;

			target.AppliedStatusEffects.Add(newStatusEffect);
		}
		
		/// <summary>
		/// Process all character's status effects.
		/// </summary>
		/// <param name="target"></param>
		public void ProcessStatusEffects(Character target)
		{
			foreach (StatusEffect statusEffect in target.AppliedStatusEffects.ToArray())
			{
				statusEffect.Tick(Time.deltaTime);
				if (statusEffect.IsFinished)
				{
					target.AppliedStatusEffects.Remove(statusEffect);
				}
			}
		}
	}
}
