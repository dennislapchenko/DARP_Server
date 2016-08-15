using System;

namespace RegionServer.Model.Items
{
    [Flags]
	public enum ItemType : byte
	{
		Weapon = 0,
		Armor = 1,
		Consumable = 2,
		Material = 3,
	}
}
