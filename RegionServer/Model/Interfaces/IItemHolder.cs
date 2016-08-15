using System.Collections.Generic;
using ComplexServerCommon.MessageObjects;

namespace RegionServer.Model.Interfaces
{
	public interface IItemHolder
	{
		CCharacter Character {get; set;}
		Dictionary<int, IItem> Inventory {get; }
		Dictionary<ItemSlot, IItem> Equipment {get;}
        IItem GetInventoryItem(int invSlot);
        IItem GetEquipmentItem(ItemSlot slot);
        IItem GetEquipmentItemById(int itemId);
		string AddItem(int itemId);
		bool RemoveItemFromInv(int invSlot);
		void RemoveAllItems();
		bool EquipItem(int invSlot);
		void EquipItemOnRestore(int itemId);
		bool DequipItem(ItemSlot slot);
		string SerializeItems();
		void DeserializeItems(string items);
	    IItem UseItem(int itemSlotNum);
	    IItem UseItem(IItem item);
	}
}