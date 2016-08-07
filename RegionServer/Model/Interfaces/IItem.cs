using System;
using ComplexServerCommon.MessageObjects;
using RegionServer.Model.Items;
using RegionServer.Model.Stats;

namespace RegionServer.Model.Interfaces
{
	public interface IItem
	{
		string Name {get; set;}
		int ItemId {get; set;}
		ItemType Type {get; set;}
		ItemSlot Slot { get; set; }

		int Value {get; set;}
		int Equippable {get; set;}
		int LevelReq { get; set; }

		IStatHolder Stats {get; set;}
	}
}