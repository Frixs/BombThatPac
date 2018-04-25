﻿using Characters;
using StatusEffects.SpecialItems;
using UnityEngine;

namespace StatusEffects.Scriptable.SpecialItems
{
    [CreateAssetMenu(menuName = "StatusEffect/SpecialItem/SlowingDoom")]
    public class ScriptableSpecialItemSlowingDoom : ScriptableStatusEffect
    {
        /// <summary>
        /// Status effect which will be applied as debuff.
        /// </summary>
        [Header("Effect Speciality")] public ScriptableMoveSpeedIncrease ApplicableStatusEffect;
        
        public override StatusEffect Initialize(Character target, Character caster)
        {
            return new SpecialItemSlowingDoom(this, target, caster);
        }
    }
}