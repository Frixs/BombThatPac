using Characters;
using Managers;
using StatusEffects.Scriptable;
using StatusEffects.Scriptable.SpecialItems;

namespace StatusEffects.SpecialItems
{
    public class SpecialItemRealityShifter : StatusEffect
    {
        public SpecialItemRealityShifter(ScriptableStatusEffect data, Character target, Character caster) : base(data, target, caster)
        {
        }

        protected override void Activate()
        {
            for (int i = 0; i < GameManager.Instance.Players.Length; i++)
            {
                if (GameManager.Instance.Players[i].CharacterInstance.GetComponent<Character>().Identifier == Target.Identifier)
                    continue;

                StatusEffectManager.Instance.ApplyStatusEffect(
                    GameManager.Instance.Players[i].CharacterInstance.GetComponent<Character>(),
                    Target,
                    ((ScriptableSpecialItemRealityShifter) Data).ApplicableStatusEffect
                );
            }
        }

        protected override void End()
        {
        }

        protected override void Repeat()
        {
            throw new System.NotImplementedException();
        }
    }
}