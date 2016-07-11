using System;
using RegionServer.Model.Interfaces;
using System.Collections.Generic;
using System.Linq;
using ComplexServerCommon;
using ExitGames.Logging;
using ComplexServerCommon.MessageObjects;

namespace RegionServer.Model.Items
{
	public class ItemHolder : IItemHolder
	{
		private static readonly bool DEBUG = true;

		public ICharacter Character {get; set;}
		private Dictionary<int, Item> _inventory;
		public Dictionary<int, Item> Inventory { get { return _inventory; } }

		private Dictionary<ItemSlot, Item> _equipment;
		public Dictionary<ItemSlot, Item> Equipment { get { return _equipment; } }

		private int _inventorySlots;
		private int _usedInventorySlots;
		public int InventorySlots { get { return _inventorySlots; } set { _inventorySlots = value; } }

		protected ILogger Log = LogManager.GetCurrentClassLogger();

		public ItemHolder(ItemDBCache itemDb)
		{
			_inventory = new Dictionary<int, Item>();
			_equipment = new Dictionary<ItemSlot, Item>();
			InventorySlots = Enum.GetNames(typeof (ItemSlot)).Length;
			_usedInventorySlots = 0;
		}

		public void SetInventorySlots(int numSlots)
		{
			_inventorySlots = numSlots;
		}
		
		public Item GetInventoryItem(int invSlot)
		{
			if(_inventory.ContainsKey(invSlot))
			{
				return _inventory[invSlot];
			}
			return null;
		}

		public Item GetEquipmentItem(ItemSlot slot)
		{
			if(_equipment.ContainsKey(slot))
			{
				return _equipment[slot];
			}
			return null;
		}
		
		public string AddItem(int itemId)
		{
			if(_usedInventorySlots < InventorySlots)
			{
				if (DEBUG) if(ItemDBCache.Items == null) Log.DebugFormat("ItemHolder - AddItem: ItemDBCache.Items == null");
				if(ItemDBCache.Items.ContainsKey(itemId))
				{
					_inventory.Add(++_usedInventorySlots, ItemDBCache.GetItem(itemId));
				}
			}
			return string.Format("+item {0}, slot {1}", ItemDBCache.GetItem(itemId).Name, _usedInventorySlots);
		}

		public bool RemoveItemFromInv(int invSlot)
		{
			if(_inventory[invSlot] != null)
			{
				if (DEBUG) Log.DebugFormat("Moved item {0} - {1} from inv to equip. removed: {2}", invSlot, _inventory[invSlot].Name,
					_inventory.Remove(invSlot));
//				_inventory.Remove(invSlot);
				_inventory = _inventory.ToDictionary(d => d.Key < invSlot ? d.Key : d.Key -1, d => d.Value); //move all below items to a +1 location
				_usedInventorySlots--;
				return true;
			}
			return false;
		}

		public void RemoveAllItems()
		{
			_inventory = new Dictionary<int, Item>();
			_equipment = new Dictionary<ItemSlot, Item>();
			_usedInventorySlots = 0;
		}
			
		public bool EquipItem(int invSlot)
		{
			var item = GetInventoryItem(invSlot);
			if (item != null)
			{
				if (DEBUG) Log.DebugFormat("Equipping item: {0} - {1}", invSlot, item.Name);
				if(item.Type != ItemType.Consumable && item.Type != ItemType.Material)
				{
					if(DequipItem(item.Slot))//remove previous item to inventory, if there was one
					{
						RemoveItemFromInv(invSlot);
						_equipment.Add(item.Slot, item);
					}
					else
					{
						RemoveItemFromInv(invSlot);
						_equipment[item.Slot] = item;
					}

					if (DEBUG) Log.DebugFormat("equipped: {0}", true);
					return true;
				}
			}
			if (DEBUG) Log.DebugFormat("equipped: {0}", false);
			return false;
		}

		//for initial loading
		public void EquipItemOnRestore(int itemId)
		{
			if(ItemDBCache.Items.ContainsKey(itemId))
			{
				var item = ItemDBCache.GetItem(itemId);
				_equipment.Add(item.Slot, item);
			}
		}

		public bool DequipItem(ItemSlot slot)
		{
			if(_equipment.ContainsKey(slot))
			{
				var item = _equipment[slot];
				if(item != null)
				{
					_usedInventorySlots = Inventory.Count;
					_equipment.Remove(slot);
					_inventory.Add(++_usedInventorySlots, item);
					return true;
				}
			}
			return false;
		}
			
		[Serializable]
		public class SerializedItem
		{
			public int ItemId {get; set;}
			public int InventorySlot {get; set;}
			public int Equipped {get; set;}
		}

		public string SerializeItems()
		{
			List<SerializedItem> ItemList = new List<SerializedItem>();
			foreach(var item in Inventory)
			{
				ItemList.Add(new SerializedItem() { ItemId = item.Value.ItemId, InventorySlot = item.Key, Equipped = 0} );
			}
			foreach(var equipped in Equipment.Values.ToList())
			{
				ItemList.Add(new SerializedItem() { ItemId = equipped.ItemId, Equipped = 1} );
			}
			return ComplexServerCommon.SerializeUtil.Serialize<List<SerializedItem>>(ItemList);
		}
		
		public void DeserializeItems(string items)
		{
			var list = ComplexServerCommon.SerializeUtil.Deserialize<List<SerializedItem>>(items);
			foreach (var item in list)
			{
				if(item.Equipped == 0)
				{
					SortInventoryItemSlotsOnLoad(item);
				}
				else
				{
					EquipItemOnRestore(item.ItemId);
				}
			}
			_usedInventorySlots = list.Count;
		}

		private void SortInventoryItemSlotsOnLoad(SerializedItem item)
		{
			if(ItemDBCache.Items.ContainsKey(item.ItemId))
			{
				Inventory.Add(item.InventorySlot, ItemDBCache.Items[item.ItemId] as Item);
			}
		}

	}
}

