using System;
using System.Collections.Generic;
using RegionServer.Model.Interfaces;
using SubServerCommon;
using ComplexServerCommon;
using SubServerCommon.Data.NHibernate;
using ExitGames.Logging;
using ComplexServerCommon.MessageObjects;

namespace RegionServer.Model.Items
{
	public class ItemDBCache
	{
		public static readonly Dictionary<int, IItem> Items = new Dictionary<int, IItem>();

		public static ItemDBCache instance;

        private Item.Factory ItemFactory { get; set; }

		protected ILogger Log = LogManager.GetCurrentClassLogger();

		public ItemDBCache(IEnumerable<IStat> stats, Item.Factory itemFactory)
		{
			instance = this;
			Log.DebugFormat("ItemDBCache constructor called");
		    ItemFactory = itemFactory;
			PullFromDB();
		}

		private void PullFromDB()
		{
			//establish one-time mysql session pipe to retrieve all item data
			using (var session = NHibernateHelper.OpenSession())
			{
				using (var transaction = session.BeginTransaction())
				{
					//pulls list of all items from SQL database into a single object
					var items = session.QueryOver<ItemDBEntry>().List();
					if (items != null)
					{
						foreach (ItemDBEntry item in items)
						{
							//builds each dbItem from SQL data to be usable in game
							var result = BuildItem(item);
							//adds built items to in-game dbItem cache
							Items.Add(result.ItemId, result);
						}
					}
                    transaction.Commit();
				}
			}
		}

		private Item BuildItem(ItemDBEntry dbItem)
		{
		    var result = ItemFactory.Invoke();
		    result.ItemId = dbItem.ItemId;
		    result.Name = dbItem.Name;
		    result.Type = (ItemType) dbItem.Type;
		    result.Slot = (ItemSlot) dbItem.Slot;
		    result.Value = dbItem.Value;
		    result.Equippable = dbItem.Equippable;
		    result.Stats = FillStats(result.Stats, dbItem);
		    result.LevelReq = dbItem.LevelReq;
		    
			//Log.DebugFormat("+built item {0}, id: {1}, stats count: {2}", result.Name, result.ItemId, result.Stats.Stats.Count);
			return result;
		}

	    private IStatHolder FillStats(IStatHolder statHolder, ItemDBEntry dbItem)
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

		public static Item GetItem(int itemId)
		{
			IItem result;
			Items.TryGetValue(itemId, out result);
			if(result != null)
			{
				return result as Item;
			}
			return null;
		}
	}
}