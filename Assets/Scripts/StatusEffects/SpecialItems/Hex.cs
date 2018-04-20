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
		/// Default collider size of the character.
		/// </summary>
		private Vector2 _defaultColliderSize;
		
		/// <summary>
		/// Default collider offset of the character.
		/// </summary>
		private Vector2 _defaultColliderOffset;
		
		/// <summary>
		/// Mouse collider size of the new form.
		/// </summary>
		private Vector2 _mouseColliderSize = new Vector2(0.35f, 0.35f);
		
		/// <summary>
		/// Mouse collider offset of the new form.
		/// </summary>
		private Vector2 _mouseColliderOffset = new Vector2(0f, -0.25f);
		
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
			_defaultColliderSize = Target.GetColliderSize();
			_defaultColliderOffset = Target.GetColliderOffset();
			Target.SetColliderSize(_mouseColliderSize.x, _mouseColliderSize.y);
			Target.SetColliderOffset(_mouseColliderOffset.x, _mouseColliderOffset.y);
			
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
			Target.SetColliderSize(_defaultColliderSize.x, _defaultColliderSize.y);
			Target.SetColliderOffset(_defaultColliderOffset.x, _defaultColliderOffset.y);
			
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
