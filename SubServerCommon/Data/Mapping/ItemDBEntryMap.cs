using FluentNHibernate.Mapping;
using SubServerCommon.Data.NHibernate;

namespace SubServerCommon.Data.Mapping
{
	public class ItemDBEntryMap : ClassMap<ItemDBEntry>
	{
		public ItemDBEntryMap()
		{
			Id(x => x.ItemId).Column("id");
			Map(x => x.Name).Column("name");
			Map(x => x.Type).Column("item_type");
			Map(x => x.Slot).Column("item_slot");
			Map(x => x.Value).Column("value");
			Map(x => x.Equippable).Column("equippable");
			Map(x => x.LevelReq).Column("level_req");
			Map(x => x.AddedStats).Column("added_stats");
			Map(x => x.minDamageValue).Column("min_damage");
			Map(x => x.maxDamageValue).Column("max_damage");
			Map(x => x.strengthValue).Column("strength");
			Map(x => x.dexterityValue).Column("dexterity");
			Map(x => x.instinctValue).Column("instinct");
			Map(x => x.staminaValue).Column("stamina");
			Map(x => x.criticalHitChanceValue).Column("critical_chance");
			Map(x => x.criticalDamageValue).Column("critical_damage");
			Map(x => x.dodgeChanceValue).Column("dodge_chance");
			Map(x => x.counterAttackChanceValue).Column("counter_attack_chance");
			Map(x => x.healthRegenValue).Column("health_regen");
			Table("items");
		}
	}
}

