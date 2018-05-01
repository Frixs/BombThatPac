﻿using Characters;
using UnityEngine;

namespace StatusEffects.Scriptable
{
    [CreateAssetMenu(menuName = "StatusEffect/ImmobileSlide")]
    public class ScriptableImmobileSlide : ScriptableStatusEffect
    {
        /// <summary>
        /// Slide speed.
        /// </summary>
        [Header("Effect Speciality")] public float SlideSpeed;
        
        /// <summary>
        /// Reference to animation of stun during slide.
        /// </summary>
        public GameObject StunAnimationPrefab;
        
        public override StatusEffect Initialize(Character target, Character caster)
        {
            return new ImmobileSlide(this, target, caster);
        }
    }
}
