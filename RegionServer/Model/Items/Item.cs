using RegionServer.Model.Interfaces;
using ComplexServerCommon.MessageObjects;
using RegionServer.Model.Stats.ItemStats;

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
		public IStatHolder Stats {get; set;}

	    public delegate Item Factory();

	    public Item(IItemStatHolder stats)
	    {
	        Stats = stats as IStatHolder;
	    }

		public static implicit operator ItemData(Item item)
		{
			//StatHolder stats = Stats;
			return new ItemData(item.Name, item.ItemId, (int)item.Type, (int)item.Slot, item.Value, item.Equippable, item.LevelReq, ((ItemStatHolder)item.Stats).GetNonNullStats());
		}
	}
}
