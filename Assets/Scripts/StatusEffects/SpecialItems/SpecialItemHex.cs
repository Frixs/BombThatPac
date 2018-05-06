using Characters;
using Characters.Effects;
using Managers;
using StatusEffects.Scriptable;
using StatusEffects.Scriptable.SpecialItems;
using UnityEngine;

namespace StatusEffects.SpecialItems
{
	public class SpecialItemHex : StatusEffect
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
		private Vector2 _mouseColliderSize = new Vector2(0.45f, 0.45f);
		
		/// <summary>
		/// Mouse collider offset of the new form.
		/// </summary>
		private Vector2 _mouseColliderOffset = new Vector2(0f, -0.25f);

		/// <summary>
		/// Reference to go back to default shadow offset.
		/// </summary>
		private Vector3 _defaultShadowOffset;
		
		public SpecialItemHex(ScriptableStatusEffect data, Character target, Character caster) : base(data, target, caster)
		{
		}

		protected override void Delay()
		{
			throw new System.NotImplementedException();
		}

		protected override void Activate()
		{
			// Aplly status effect immune form.
			Target.IsStatusEffectImmune = true;
			
			// Create an morph explosion.
			SpawnManager.Instance.SpawnAnimationAtPositionWithExpiry(((ScriptableSpecialItemHex) Data).ExplosionPrefab, Target.transform.position, Quaternion.identity);
			
			// Add mouse form.
			Target.MyAnimator.runtimeAnimatorController = ((ScriptableSpecialItemHex) Data).MouseController;
			_defaultColliderSize = Target.GetColliderSize();
			_defaultColliderOffset = Target.GetColliderOffset();
			Target.SetColliderSize(_mouseColliderSize.x, _mouseColliderSize.y);
			Target.SetColliderOffset(_mouseColliderOffset.x, _mouseColliderOffset.y);
			_defaultShadowOffset = Target.GetComponent<BlobShadowEffect>().Offset; 
			Target.GetComponent<BlobShadowEffect>().Offset = new Vector3(0f, -0.4f, 0f);
			
			// Add movespeed.
			Target.MoveSpeed += ((ScriptableSpecialItemHex) Data).MoveSpeedIncrease;
			
			// Play sound.
			SoundManager.Instance.PlaySingleSfx(((ScriptableSpecialItemHex) Data).StartSfx);
			
			// Switch off bomb-planting.
			((Player) Target).CanPlantBombs = false;
		}

		protected override void End()
		{
			// Create an morph explosion.
			SpawnManager.Instance.SpawnAnimationAtPositionWithExpiry(((ScriptableSpecialItemHex) Data).ExplosionPrefab, Target.transform.position, Quaternion.identity);
			
			// Return default character form.
			Target.MyAnimator.runtimeAnimatorController = Target.AnimationControllerDefault;
			Target.SetColliderSize(_defaultColliderSize.x, _defaultColliderSize.y);
			Target.SetColliderOffset(_defaultColliderOffset.x, _defaultColliderOffset.y);
			Target.GetComponent<BlobShadowEffect>().Offset = _defaultShadowOffset;
			
			// Return movespeed.
			Target.MoveSpeed -= ((ScriptableSpecialItemHex) Data).MoveSpeedIncrease;
			
			// Play sound.
			SoundManager.Instance.PlaySingleSfx(((ScriptableSpecialItemHex) Data).EndSfx);
			
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
