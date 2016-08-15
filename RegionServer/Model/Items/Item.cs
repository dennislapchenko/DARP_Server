using System;
using RegionServer.Model.Interfaces;
using ComplexServerCommon.MessageObjects;
using RegionServer.Model.Effects.Definitions;

namespace RegionServer.Model.Items
{
	public class Item : IItem
	{
		public string Name {get; set;}
	    public string Description { get; set; }
	    public int ItemId {get; set;}
	    public ItemType Type {get; set;}
		public ItemSlot Slot {get; set;}
		public int Value {get; set;}
	    public byte MaxStack { get; set; }
		public int LevelReq {get;set;}
	    public IEffect Effect { get; set; }

	    public delegate Item Factory();

	    public Item()
	    {

	    }

		public static implicit operator ItemData(Item item)
		{
		    if (item is EquipmentItem)
		    {
                return new ItemData(item.Name, item.Description, item.ItemId, (int)item.Type, (int)item.Slot, item.Value,
                         item.MaxStack,
                             item.LevelReq, item.Effect.EnumId.ToString(), ((EquipmentItem)item).Stats.GetNonNullStats());
            }
            return new ItemData(item.Name, item.Description, item.ItemId, (int)item.Type, (int)item.Slot, item.Value, item.MaxStack,
		            item.LevelReq, item.Effect.EnumId.ToString());
		}
	}
}
