using System;
using RegionServer.Model.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ComplexServerCommon;
using ExitGames.Logging;
using ComplexServerCommon.MessageObjects;
using RegionServer.DataHelperObjects;

namespace RegionServer.Model.Items
{
	public class ItemHolder : IItemHolder
	{
		private static readonly bool DEBUG = true;

		public CCharacter Character {get; set;}


	    private Dictionary<ItemType, List<IItem>> Inventories;
		private Dictionary<int, IItem> _inventory;
		public Dictionary<int, IItem> Inventory { get { return _inventory; } }

		private Dictionary<ItemSlot, IItem> _equipment;
		public Dictionary<ItemSlot, IItem> Equipment { get { return _equipment; } }

	    private CappedByte inventorySpace;
		private int _inventorySlots;
		private int _usedInventorySlots;
		public int InventorySlots { get { return _inventorySlots; } set { _inventorySlots = value; } }

		protected ILogger Log = LogManager.GetCurrentClassLogger();

		public ItemHolder(ItemDBCache itemDb)
		{
		    Inventories = new Dictionary<ItemType, List<IItem>>
		    {
		        {ItemType.Armor, new List<IItem>()},
		        {ItemType.Consumable, new List<IItem>()},
		        {ItemType.Material, new List<IItem>()}
		    };
            inventorySpace = CappedByte.Init(0, byte.MaxValue);


		    _inventory = new Dictionary<int, IItem>();
			_equipment = new Dictionary<ItemSlot, IItem>();
			InventorySlots = Enum.GetNames(typeof (ItemSlot)).Length;
			_usedInventorySlots = 0;
		}

		public void SetInventorySlots(int numSlots)
		{
			_inventorySlots = numSlots;
		}
		
		public IItem GetInventoryItem(int invSlot)
		{
		    IItem item;
		    _inventory.TryGetValue(invSlot, out item);
		    return item;
		}

        public IItem GetInventoryItem(IItem item)
        {
            IItem result = _inventory.Values.FirstOrDefault(x => x.ItemId == item.ItemId);
            return result; //returns Item if found or null if not found
        }

        public IItem GetEquipmentItem(ItemSlot slot)
        {
            IItem item;
            _equipment.TryGetValue(slot, out item);
            return item;
		}

	    public IItem GetEquipmentItemById(int itemId)
	    {
            IItem item = _inventory.Values.FirstOrDefault(x => x.ItemId == itemId);
	        return item;
	    }

	    public string AddItem(int itemId)
		{
			if(_usedInventorySlots < InventorySlots)
			{
			    IItem item = ItemDBCache.GetItem(itemId);
				if(item != null)
				{
				    if (item.Type == ItemType.Weapon || item.Type == ItemType.Armor)
				    {
					    _inventory.Add(++_usedInventorySlots, item);
				    }
				    else
				    {
				        
				    }
				}
			}
			return string.Format("+item {0}, slot {1}", ItemDBCache.GetItem(itemId).Name, _usedInventorySlots);
		}

        public string AddItemNEW(int itemId)
        {
            if (inventorySpace.Value < inventorySpace.GetCap())
            {
                IItem item = ItemDBCache.GetItem(itemId);
                if (item != null)
                {
                    switch (item.Type)
                    {
                        case ItemType.Weapon:
                        case ItemType.Armor:
                            Inventories[ItemType.Armor].Add(item);
                            break;
                        case ItemType.Consumable:
                            Inventories[ItemType.Consumable].Add(item);
                            break;
                        case ItemType.Material:
                            Inventories[ItemType.Material].Add(item);
                            break;
                        default: throw new InvalidEnumArgumentException("Unknown itemType enum passed to ItemHolder::AddNewItem");
                    }
                    inventorySpace.PutOne();
                }
            }
            return string.Format("+item {0}, slot {1}", ItemDBCache.GetItem(itemId).Name, inventorySpace.Value-1);
        }

        public bool RemoveItemFromInv(int invSlot)
		{
			if(_inventory.ContainsKey(invSlot))
			{
				if (DEBUG) Log.DebugFormat("Moved item {0} - {1} from inv to equip.", invSlot, _inventory[invSlot].Name);
				_inventory.Remove(invSlot);
				_inventory = _inventory.ToDictionary(d => d.Key < invSlot ? d.Key : d.Key -1, d => d.Value); //move all below items to a +1 location
				_usedInventorySlots--;
				return true;
			}
			return false;
		}

		public void RemoveAllItems()
		{
			_inventory = new Dictionary<int, IItem>();
			_equipment = new Dictionary<ItemSlot, IItem>();
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
		    if (!ItemDBCache.Items.ContainsKey(itemId)) return;

		    var item = ItemDBCache.GetItem(itemId);
		    if (_equipment.ContainsKey(item.Slot))
		    {
		        _equipment[item.Slot] = item;
		    }
		    else
		    {
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

        public IItem UseItem(int itemSlotNum)
        {
            IItem item = GetInventoryItem(itemSlotNum);
            var consumable = item as Item;
            if (consumable != null)
            {
                Character.Effects.Apply(consumable.Effect);
            }
            return consumable;
        }

        public IItem UseItem(IItem item)
        {
            return null;
        }

        private void SortInventoryItemSlotsOnLoad(SerializedItem item)
		{
			if(ItemDBCache.Items.ContainsKey(item.ItemId))
			{
				Inventory.Add(item.InventorySlot, ItemDBCache.Items[item.ItemId] as Item);
			}
		}

        #region SERIALIZATION
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
			return SerializeUtil.Serialize(ItemList);
		}
		
		public void DeserializeItems(string items)
		{
			var list = SerializeUtil.Deserialize<List<SerializedItem>>(items);
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
        #endregion
	}
}

