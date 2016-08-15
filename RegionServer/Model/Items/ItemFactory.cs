using System;
using System.Collections.Generic;
using ComplexServerCommon;
using ComplexServerCommon.MessageObjects;
using ExitGames.Logging;
using RegionServer.Model.Effects;
using RegionServer.Model.Interfaces;
using RegionServer.Model.Stats.ItemStats;
using SubServerCommon.Data.NHibernate;

namespace RegionServer.Model.Items
{
    public class ItemFactory
    {
        protected static ILogger Log = LogManager.GetCurrentClassLogger();

        private readonly Item.Factory _itemFactory;
        private readonly EquipmentItem.EqFactory _eqItemFactory;

        public ItemFactory(Item.Factory itemFac, EquipmentItem.EqFactory eqItemFac)
        {
            _itemFactory = itemFac;
            _eqItemFactory = eqItemFac;
        }


        public IItem BuildItem(ItemDBEntry dbItem)
        {
            IItem result;
            switch ((ItemType)dbItem.Type)
            {
                case ItemType.Armor: //fall through is intended, to catch armor or weapon in same case (same below with cons&mat)
                case ItemType.Weapon:
                    result = _eqItemFactory.Invoke();
                    ((EquipmentItem) result).Stats = FillStats(((EquipmentItem) result).Stats, dbItem);
                    break;
                case ItemType.Consumable:
                case ItemType.Material:
                    result = _itemFactory.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("ItemFactory::BuildItem - unexpected ItemType enum");
            }

            result.Name         = dbItem.Name;
            result.Description  = dbItem.Description;
            result.ItemId       = dbItem.ItemId;
            result.Type         = (ItemType)dbItem.Type;
            result.Slot         = (ItemSlot)dbItem.Slot;
            result.Value        = dbItem.Value;
            result.MaxStack     = dbItem.MaxStack;
            result.LevelReq     = dbItem.LevelReq;
            result.Effect       = EffectCache.GetEffect((EffectEnum) dbItem.Effect);
            Log.DebugFormat("+built item {0}, id: {1}, item type: {2}, item slot: {3}, num stats: {4}, effect: {5}",
                result.Name, result.ItemId, result.Type, result.Slot, ((((result as EquipmentItem) != null) ? ((EquipmentItem)result).Stats.Stats.Count+"" : "not stat item")), result.Effect.Name);

            return result;
        }

        private ItemStatHolder FillStats(ItemStatHolder statHolder, ItemDBEntry dbItem)
        {
            var statsDict = SerializeUtil.Deserialize<Dictionary<string, float>>(dbItem.Stats);

            var statsCopy = new Dictionary<Type, IStat>(statHolder.Stats);
            foreach (var stat in statsCopy)
            {
                if (statsDict != null && statsDict.ContainsKey(stat.Value.Name) && statsDict[stat.Value.Name] > 0)
                {
                    ((IItemStatHolder)statHolder).SetStat(stat.Value, statsDict[stat.Value.Name]);
                }
                else
                {
                    statHolder.Stats.Remove(stat.Key);
                }
            }
            return statHolder;
        }
    }
}