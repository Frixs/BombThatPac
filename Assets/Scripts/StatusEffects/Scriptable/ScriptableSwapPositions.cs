using Characters;
using UnityEngine;

namespace StatusEffects.Scriptable
{
	[CreateAssetMenu(menuName = "StatusEffect/SwapPositions")]
	public class ScriptableSwapPositions : ScriptableStatusEffect
	{
		/// <summary>
		/// Speed of the teleport (changing positions).
		/// </summary>
		[Header("Effect Speciality")] public float TeleportSpeed;
		
		/// <summary>
		/// First explosion on hide / before teleporting.
		/// </summary>
		public GameObject StartEffectExplosionPrefab;
		
		/// <summary>
		/// Teleporting particle trail.
		/// </summary>
		public GameObject TrailPrefab;
		
		/// <summary>
		/// End explosion after teleporting.
		/// </summary>
		public GameObject EndEffectExplosionPrefab;
		
		/// <summary>
		/// Sound on start.
		/// </summary>
		public AudioClip StartSfx;
		
		/// <summary>
		/// Sound on trail casting.
		/// </summary>
		public AudioClip TrailSfx;
		
		/// <summary>
		/// Sound on end.
		/// </summary>
		public AudioClip EndSfx;
		
		public override StatusEffect Initialize(Character target, Character caster)
		{
			return new SwapPositions(this, target, caster);
		}
	}
}
