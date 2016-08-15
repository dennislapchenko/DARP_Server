using ComplexServerCommon.MessageObjects;
using RegionServer.Model.Effects.Definitions;
using RegionServer.Model.Items;

namespace RegionServer.Model.Interfaces
{
	public interface IItem
	{
		string Name {get; set;}
        string Description { get; set; }
		int ItemId {get; set;}
		int Value {get; set;}
        byte MaxStack { get; set; }
		ItemType Type {get; set;}
		ItemSlot Slot { get; set; }
		int LevelReq { get; set; }

		IEffect Effect { get; set; }
    }
}