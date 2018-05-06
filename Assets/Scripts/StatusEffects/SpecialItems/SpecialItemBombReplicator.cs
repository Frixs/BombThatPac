using System;
using Characters;
using Effects;
using Managers;
using StatusEffects.Scriptable;
using StatusEffects.Scriptable.SpecialItems;
using UnityEngine;

namespace StatusEffects.SpecialItems
{
    public class SpecialItemBombReplicator : StatusEffect
    {
        /// <summary>
        /// Reference to following animation
        /// </summary>
        private GameObject _followingAnimation;

        public SpecialItemBombReplicator(ScriptableStatusEffect data, Character target, Character caster) : base(data, target, caster)
        {
        }

        protected override void Delay()
        {
            throw new NotImplementedException();
        }

        protected override void Activate()
        {
            // Spawn animation.
            SmokeEffect anim = Target.GetComponentInChildren<SmokeEffect>();
            if (anim == null)
                _followingAnimation =
                    SpawnManager.Instance.SpawnFollowingAnimationLoop(((ScriptableSpecialItemBombReplicator) Data).BuffEffectPrefab, Target.gameObject, new Vector3(0.4f, 0.7f, 0f), Quaternion.identity);
            else
                _followingAnimation = anim.gameObject;

            // Edit number in the smoke animation.
            _followingAnimation.GetComponent<SmokeEffect>().TextLabel.text =
                "" + (Int32.Parse(_followingAnimation.GetComponent<SmokeEffect>().TextLabel.text) + ((ScriptableSpecialItemBombReplicator) Data).AdditionalBombs);

            // Apply status effect.
            ((Player) Target).BombMaxAllowedDeploys += ((ScriptableSpecialItemBombReplicator) Data).AdditionalBombs;

            // Play sound.
            SoundManager.Instance.PlaySingleSfx(((ScriptableSpecialItemBombReplicator) Data).StartSfx);
        }

        protected override void End()
        {
            // Remove status effect.
            ((Player) Target).BombMaxAllowedDeploys -= ((ScriptableSpecialItemBombReplicator) Data).AdditionalBombs;

            // Edit number in the smoke animation.
            _followingAnimation.GetComponent<SmokeEffect>().TextLabel.text =
                "" + (Int32.Parse(_followingAnimation.GetComponent<SmokeEffect>().TextLabel.text) - ((ScriptableSpecialItemBombReplicator) Data).AdditionalBombs);

            // Despawn animation.
            if (Int32.Parse(_followingAnimation.GetComponent<SmokeEffect>().TextLabel.text) == 0) // 0 is default value in the text label.
                SpawnManager.Instance.DespawnAnimation(_followingAnimation);
        }

        protected override void Repeat()
        {
            throw new System.NotImplementedException();
        }
    }
}