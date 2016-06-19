using System.Collections.Generic;
using System;
using RegionServer.Model.Items;
using ComplexServerCommon.MessageObjects;

namespace RegionServer.Model.Interfaces
{
	public interface IItemHolder
	{
		ICharacter Character {get; set;}
		Dictionary<int, Item> Inventory {get; }
		Dictionary<ItemSlot, Item> Equipment {get;}
//		IItem GetItem<T>() where T : class, IItem;
//		IItem GetItem<T>(T item) where T : class, IItem;
		Item GetInventoryItem(int itemId);
		Item GetEquipmentItem(ItemSlot slot);
		string AddItem(int itemId);
		bool RemoveItemFromInv(int invSlot);
		void RemoveAllItems();
		bool EquipItem(int invSlot);
		void EquipItemOnRestore(int itemId);
		bool DequipItem(ItemSlot slot);
		string SerializeItems();
		void DeserializeItems(string items);
	}
}