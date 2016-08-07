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
		    Map(x => x.Stats).Column("Stats");
			Table("items");
		}
	}
}

