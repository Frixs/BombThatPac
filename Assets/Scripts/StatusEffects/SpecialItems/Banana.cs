using Characters;
using Items;
using Managers;
using StatusEffects.Scriptable;
using StatusEffects.Scriptable.SpecialItems;
using UnityEngine;

namespace StatusEffects.SpecialItems
{
	public class Banana : StatusEffect
	{
		public Banana(ScriptableStatusEffect data, Character target, Character caster) : base(data, target, caster)
		{
		}

		protected override void Activate()
		{
			Vector3Int cell = MapManager.Instance.TilemapGameplay.WorldToCell(Target.transform.position);
			Vector3 cellCenter = MapManager.Instance.TilemapGameplay.GetCellCenterWorld(cell);
			
			ItemBananaPeel.Spawn(((ScriptableBanana) Data).BananaPeelPrefab, cellCenter, Quaternion.identity, Target.GetComponent<Player>());
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
