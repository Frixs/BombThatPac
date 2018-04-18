using Characters;
using Managers;
using StatusEffects.Scriptable;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace StatusEffects
{
	public class ImmobileSlide : StatusEffect
	{
		public ImmobileSlide(ScriptableStatusEffect data, Character target, Character caster) : base(data, target, caster)
		{
		}

		protected override void Activate()
		{
			Target.DisableActions();
			Target.MyRigidBody.AddForce(Target.transform.forward.normalized * ((ScriptableImmobileSlide) Data).SlideSpeed);
			Target.GetComponent<SpriteRenderer>().color = new Color(1f, 0.4f, 0.4f);
		}

		protected override void End()
		{
			Target.EnableActions();
			Target.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
		}

		protected override void Repeat()
		{
		}
	}
}
