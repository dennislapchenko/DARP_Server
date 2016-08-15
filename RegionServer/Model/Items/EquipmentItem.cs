using RegionServer.Model.Interfaces;
using ComplexServerCommon.MessageObjects;
using RegionServer.Model.Stats.ItemStats;

namespace RegionServer.Model.Items
{
    public class EquipmentItem : Item
    {
//        public string Name { get; set; }
//        public string Description { get; set; }
//        public int ItemId { get; set; }
//        public byte MaxStack { get; set; }
//        public ItemType Type { get; set; }
//        public ItemSlot Slot { get; set; }
//        public int Value { get; set; }
//        public int LevelReq { get; set; }
//        public IEffect Effect { get; set; }
        public ItemStatHolder Stats { get; set; }

        public delegate EquipmentItem EqFactory();

        public EquipmentItem(IItemStatHolder stats)
        {
            Stats = stats as ItemStatHolder;
        }

        public static implicit operator ItemData(EquipmentItem item)
        {
            return new ItemData(item.Name, item.Description, item.ItemId, (int) item.Type, (int) item.Slot, item.Value,
                item.MaxStack,
                item.LevelReq, item.Effect.EnumId.ToString(), item.Stats.GetNonNullStats());
        }
    }
}

