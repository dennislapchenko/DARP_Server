using System;
using RegionServer.Model.Items;
using RegionServer.Model.Stats;

namespace RegionServer.Model.Interfaces
{
	public interface IItem
	{
		string Name {get; set;}
		int ItemId {get; set;}
		ItemType Type {get; set;}

		int Value {get; set;}
		int Equippable {get; set;}
		StatHolder Stats {get; set;}
	}
}