using Characters;
using Managers;
using StatusEffects.Scriptable;
using StatusEffects.Scriptable.SpecialItems;
using UnityEngine;

namespace StatusEffects.SpecialItems
{
	public class Hex : StatusEffect
	{
		/// <summary>
		/// Default collider radius of the character.
		/// </summary>
		private float _defaultColliderRadius;
		
		/// <summary>
		/// Mouse collider radius of the new form.
		/// </summary>
		private float _mouseColliderRadius = 0.25f;
		
		public Hex(ScriptableStatusEffect data, Character target, Character caster) : base(data, target, caster)
		{
		}

		protected override void Activate()
		{
			// Aplly status effect immune form.
			Target.IsStatusEffectImmune = true;
			
			// Create an morph explosion.
			SpawnManager.Instance.SpawnAnimationAtPosition(((ScriptableHex) Data).ExplosionPrefab, Target.transform.position, Quaternion.identity);
			
			// Add mouse form.
			Target.MyAnimator.runtimeAnimatorController = ((ScriptableHex) Data).MouseController;
			_defaultColliderRadius = Target.GetComponent<CircleCollider2D>().radius;
			Target.GetComponent<CircleCollider2D>().radius = _mouseColliderRadius;
			
			// Add movespeed.
			Target.MoveSpeed += ((ScriptableHex) Data).MoveSpeedIncrease;
			
			// Switch off bomb-planting.
			((Player) Target).CanPlantBombs = false;
		}

		protected override void End()
		{
			// Create an morph explosion.
			SpawnManager.Instance.SpawnAnimationAtPosition(((ScriptableHex) Data).ExplosionPrefab, Target.transform.position, Quaternion.identity);
			
			// Return default character form.
			Target.MyAnimator.runtimeAnimatorController = Target.AnimationControllerDefault;
			Target.GetComponent<CircleCollider2D>().radius = _defaultColliderRadius;
			
			// Return movespeed.
			Target.MoveSpeed -= ((ScriptableHex) Data).MoveSpeedIncrease;
			
			// Switch on bomb-planting.
			((Player) Target).CanPlantBombs = true;
			
			// Remove status effect immune form.
			Target.IsStatusEffectImmune = false;
		}

		protected override void Repeat()
		{
		}
	}
}
