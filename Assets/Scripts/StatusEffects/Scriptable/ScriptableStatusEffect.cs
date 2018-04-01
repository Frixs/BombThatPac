using Characters;
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
		/// Duration of the effect.
		/// </summary>
		[Header("Effect Property")] public float Duration;
		
		/// <summary>
		/// Is this status effect stackable with the same type?
		/// </summary>
		public bool IsStackable;
		
		/// <summary>
		/// Can this status effect be overwritten by the same type of the status effect?
		/// </summary>
		public bool CanBeOverwritten;
		
		/// <summary>
		/// Delay from the start of the status effect to be applied.
		/// </summary>
		public float StartActivationDelay;
		
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
