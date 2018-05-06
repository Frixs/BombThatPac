using Characters;
using Managers;
using StatusEffects.Scriptable;
using UnityEngine;

namespace StatusEffects
{
	public class SwapPositions : StatusEffect
	{
		private GameObject _trailCaster = null;
		private GameObject _trailAnotherPlayer = null;
		private Player _anotherPlayer = null;
		private GameObject _trailSfx = null;
		private GameObject _delaySfx = null;
		private GameObject _delayEffectCaster = null;
		
		public SwapPositions(ScriptableStatusEffect data, Character target, Character caster) : base(data, target, caster)
		{
		}

		protected override void Delay()
		{
			// Spawn delay animation.
			_delayEffectCaster = SpawnManager.Instance.SpawnFollowingAnimationLoop(((ScriptableSwapPositions) Data).DelayEffectPrefab, Caster.gameObject, Vector3.zero, Quaternion.identity);
			
			// Play sound.
			_delaySfx = SoundManager.Instance.PlaySingleSfx(((ScriptableSwapPositions) Data).DelaySfx);
		}

		protected override void Activate()
		{
			SoundManager.Instance.StopPlayingSfx(_delaySfx);
			SpawnManager.Instance.DespawnAnimation(_delayEffectCaster);
			
			// Find random player to change position with.
			float theLongestDistance = 0f;
			foreach (PlayerManager pm in GameManager.Instance.Players)
			{
				if (Caster.Identifier == pm.PlayerComponent.Identifier)
					continue;
				
				// Get distance with another player.
				float distance = Vector2.Distance(Caster.transform.position, pm.CharacterInstance.transform.position);
				
				// Find the distance from our target player to the most far player.
				if (distance > theLongestDistance)
				{
					theLongestDistance = distance;
					_anotherPlayer = pm.PlayerComponent;
				}
			}

			// Check if the player exists.
			if (_anotherPlayer == null)
				return;
			
			// Check if the player is alive or is immune to status effects.
			if (_anotherPlayer.IsDeath || _anotherPlayer.IsStatusEffectImmune)
				return;
			
			// Spawn explosion animation.
			SpawnManager.Instance.SpawnAnimationAtPosition(((ScriptableSwapPositions) Data).StartEffectExplosionPrefab, Caster.transform.position, Quaternion.identity);
			SpawnManager.Instance.SpawnAnimationAtPosition(((ScriptableSwapPositions) Data).StartEffectExplosionPrefab, _anotherPlayer.transform.position, Quaternion.identity);
			
			// Set ínactive both players.
			Caster.gameObject.SetActive(false);
			_anotherPlayer.gameObject.SetActive(false);
			Caster.IsStatusEffectImmune = true;
			_anotherPlayer.IsStatusEffectImmune = true;
			
			// Play sound.
			SoundManager.Instance.PlaySingleSfx(((ScriptableSwapPositions) Data).StartSfx);
			_trailSfx = SoundManager.Instance.PlaySingleSfx(((ScriptableSwapPositions) Data).TrailSfx);
			
			
			// Instantiate trail prefab.
			_trailCaster = SpawnManager.Instance.SpawnAnimationAtPosition(((ScriptableSwapPositions) Data).TrailPrefab, Caster.transform.position, Quaternion.identity);
			_trailAnotherPlayer = SpawnManager.Instance.SpawnAnimationAtPosition(((ScriptableSwapPositions) Data).TrailPrefab, _anotherPlayer.transform.position, Quaternion.identity);
			
			// Add trails to camera target for better view.
			for (int i = 0; i < GameManager.Instance.CameraControl.Targets.Length; i++)
			{
				if (GameManager.Instance.CameraControl.Targets[i] == Caster.transform)
					GameManager.Instance.CameraControl.Targets[i] = _trailCaster.transform;
				if (GameManager.Instance.CameraControl.Targets[i] == _anotherPlayer.transform)
					GameManager.Instance.CameraControl.Targets[i] = _trailAnotherPlayer.transform;
			}
			
			// Transform the trails.
			TransformManager.Instance.AddTransfMoveTowards(_trailCaster, 0, _anotherPlayer.transform.position, ((ScriptableSwapPositions) Data).TeleportSpeed);
			TransformManager.Instance.AddTransfMoveTowards(_trailAnotherPlayer, 0, Caster.transform.position, ((ScriptableSwapPositions) Data).TeleportSpeed);
		}

		protected override void End()
		{
		}

		protected override void Repeat()
		{
			// Check if trails has benn already sent.
			if (_trailCaster == null || _trailAnotherPlayer == null)
				return;
			
			// Check if trails are on its final positions.
			if (_trailCaster.transform.position == _anotherPlayer.transform.position && _trailAnotherPlayer.transform.position == Caster.transform.position)
			{
				// Spawn explosion animation.
				SpawnManager.Instance.SpawnAnimationAtPosition(((ScriptableSwapPositions) Data).StartEffectExplosionPrefab, Caster.transform.position, Quaternion.identity);
				SpawnManager.Instance.SpawnAnimationAtPosition(((ScriptableSwapPositions) Data).StartEffectExplosionPrefab, _anotherPlayer.transform.position, Quaternion.identity);
				
				// Play sound.
				SoundManager.Instance.StopPlayingSfx(_trailSfx);
				SoundManager.Instance.PlaySingleSfx(((ScriptableSwapPositions) Data).EndSfx);
				
				SpawnManager.Instance.DespawnAnimation(_trailCaster);
				SpawnManager.Instance.DespawnAnimation(_trailAnotherPlayer);
				
				Caster.transform.position = _trailCaster.transform.position;
				_anotherPlayer.transform.position = _trailAnotherPlayer.transform.position;
				
				// Get back camera targets.
				for (int i = 0; i < GameManager.Instance.CameraControl.Targets.Length; i++)
				{
					if (GameManager.Instance.CameraControl.Targets[i] == _trailCaster.transform)
						GameManager.Instance.CameraControl.Targets[i] = Caster.transform;
					if (GameManager.Instance.CameraControl.Targets[i] == _trailAnotherPlayer.transform)
						GameManager.Instance.CameraControl.Targets[i] = _anotherPlayer.transform;
				}
				
				Caster.gameObject.SetActive(true);
				_anotherPlayer.gameObject.SetActive(true);
				Caster.IsStatusEffectImmune = false;
				_anotherPlayer.IsStatusEffectImmune = false;
			}
		}
	}
}
