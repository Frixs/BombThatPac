﻿using Characters;
using UnityEngine;

namespace StatusEffects.Scriptable
{
	/// <summary>
	/// This class holds all global variables needs to be in every status effects.
	/// All additional variables should be defined in inherited class special for each status effect.
	/// </summary>
	public abstract class ScriptableStatusEffect : ScriptableObject
	{
		/// <summary>
		/// Type of the current status effect.
		/// </summary>
		[Header("Basic Settings")] public StatusEffect.StatusEffectType StatusEffectType;
		
		/// <summary>
		/// Should be the status effect permanent? If yes, Duration parameter is irrelevant.
		/// </summary>
		[Header("Effect Duration")] public bool IsPermanent;
		
		/// <summary>
		/// Duration of the effect. If this is set to 0 then the status effect will be permanent till manually rmeoving the effect.
		/// </summary>
		public float Duration;

		/// <summary>
		/// Should the status effect be removed at death of the target?
		/// </summary>
		public bool RemoveAtDeath;

		/// <summary>
		/// Should this status effect overwrite all status effects of that type (like MoveSPeedIncrease)? If this is TRUE then OverwriteTheSameEffects & IsStackable is irrelevant.
		/// </summary>
		[Header("Effect Connections")] public bool OverwriteTheSameTypes;
		
		/// <summary>
		/// Should this status effect overwrite all status effects the same as this one? If this is TRUE then IsStackable is irrelevant.
		/// </summary>
		public bool OverwriteTheSameEffects;
		
		/// <summary>
		/// Is this status effect stackable with the same type?
		/// </summary>
		public bool IsStackable;
		
		/// <summary>
		/// Delay from the start of the status effect to be applied.
		/// </summary>
		[Header("Effect Timing")] public float StartActivationDelay;
		
		/// <summary>
		/// Is this status effect damage over time (DoT)? If no, set this value to 0. If this value is set it says in which period the status effect should be activated.
		/// </summary>
		public float RepeatTime;

		/// <summary>
		/// Initialize the effect on the game object.
		/// </summary>
		/// <param name="target">Target game object to cast the effect.</param>
		/// <param name="caster">Caster game object which casted the status effect.</param>
		/// <returns>Reference of the effect.</returns>
		public abstract StatusEffect Initialize(Character target, Character caster);
	}
}
