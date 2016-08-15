using System.Collections.Generic;
using RegionServer.Model.Interfaces;
using SubServerCommon;
using SubServerCommon.Data.NHibernate;
using ExitGames.Logging;

namespace RegionServer.Model.Items
{
	public class ItemDBCache
	{
		public static readonly Dictionary<int, IItem> Items = new Dictionary<int, IItem>();

		public static ItemDBCache Instance;
	    private readonly ItemFactory _itemFactory;

		protected ILogger Log = LogManager.GetCurrentClassLogger();

		public ItemDBCache(ItemFactory itemFactory)
		{
			Instance = this;
		    _itemFactory = itemFactory;

		    var items = PullFromDB();
            foreach (ItemDBEntry item in items)
            {
                var result = _itemFactory.BuildItem(item); //builds each dbItem from SQL data to be usable in game
                Items.Add(result.ItemId, result); //adds built items to in-game dbItem cache
            }
        }

		private IEnumerable<ItemDBEntry> PullFromDB()
		{
		    IEnumerable<ItemDBEntry> result;
			//establish one-time mysql session pipe to retrieve all item data
			using (var session = NHibernateHelper.OpenSession())
			{
				using (var transaction = session.BeginTransaction())
				{
					//pulls list of all items from SQL database into a single object
					result = session.QueryOver<ItemDBEntry>().List();
                    transaction.Commit();
				}
			}
		    return result;
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