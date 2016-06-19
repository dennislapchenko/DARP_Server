using RegionServer.Model.Interfaces;
using RegionServer.Model.Stats;
using System.Collections.Generic;
using ComplexServerCommon.MessageObjects;

namespace RegionServer.Model.Items
{
	public class Item : IItem
	{
		public string Name {get; set;}
		public int ItemId {get; set;}
		public ItemType Type {get; set;}
		public ItemSlot Slot {get; set;}
		public int Value {get; set;}
		public int Equippable {get; set;}
		public int LevelReq {get;set;}
		public Dictionary<int, int> AddedStats {get; set;}
		public StatHolder Stats {get; set;}

		public static implicit operator ItemData(Item item)
		{
			//StatHolder stats = Stats;
			return new ItemData(item.Name, item.ItemId, (int)item.Type, (int)item.Slot, item.Value, item.Equippable, item.LevelReq, item.Stats.GetAllStats());
		}
	}
}
