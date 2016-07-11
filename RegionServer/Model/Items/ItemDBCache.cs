 using System;
using System.Collections.Generic;
using RegionServer.Model.Interfaces;
using SubServerCommon;
using System.Linq;
using RegionServer.Model.Stats;
using RegionServer.Calculators;
using SubServerCommon.Data.NHibernate;
using ExitGames.Logging;
using ComplexServerCommon.MessageObjects;

namespace RegionServer.Model.Items
{
	public class ItemDBCache
	{
		public static Dictionary<int, IItem> Items;

		public static ItemDBCache instance;

		private List<IStat> _allStatsList;

		protected ILogger Log = LogManager.GetCurrentClassLogger();

		public ItemDBCache(IEnumerable<IStat> stats)
		{
			instance = this;
			Log.DebugFormat("ItemDBCache constructor called");
			Items = new Dictionary<int, IItem>();
			_allStatsList = stats.ToList();
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
							//builds each item from SQL data to be usable in game
							var result = BuildItem(item);
							//adds built items to in-game item cache
							Items.Add(result.ItemId, result);
						}
					}
				}
			}
		}

		private Item BuildItem(ItemDBEntry item)
		{
			//List<IStat> statsList = new List<IStat>();
			Dictionary<int, int> addedStats = ParseStatsString(item.AddedStats);
			var result = new Item()
								{
									ItemId = item.ItemId,
									Name = item.Name,
									Type = (ItemType)item.Type,
									Slot = (ItemSlot)item.Slot,
									Value = item.Value,
									Equippable = item.Equippable,
									AddedStats = ParseStatsString(item.AddedStats), //passes 101010 string from DB item obj to convert into <int,int>Dict
									Stats = SetupItemStatCollection(BuildStatsList(addedStats, item)),//
									LevelReq = item.LevelReq,
								};
			//Log.DebugFormat("+built item {0}, id: {1}", result.Name, result.ItemId);
			return result;
		}

		private Dictionary<int, int> ParseStatsString(string input)
		{
			var charList = new List<int>();

			foreach(char ch in input)
			{
				charList.Add(int.Parse(ch.ToString()));
			}

			var resultDict = new Dictionary<int,int>();

			for (int i = 0; i < charList.Count; i++)
			{
				if (charList[i] != 0)
				{
					resultDict.Add(i, charList[i]);
				}
			}
			return resultDict;
		}

		private List<IStat> BuildStatsList(Dictionary<int, int> statsInclusionList, ItemDBEntry item)
		{
			var result = new List<IStat>();
			foreach(var stat in statsInclusionList)
			{
				//Log.DebugFormat("stat.Key: {0},  stat.Value: {1}", stat.Key, stat.Value);
				IStat statEntry;
				switch (stat.Key)
				{
					case(0):
						statEntry = _allStatsList.Where(s => s.Name == "Min Damage").FirstOrDefault() as MinDamage;
						statEntry.ConvertToIsOnItem(item.minDamageValue);
						result.Add(statEntry);
						break;
					case(1):
						statEntry = _allStatsList.Where(s => s.Name == "Max Damage").FirstOrDefault() as MaxDamage;
						statEntry.ConvertToIsOnItem(item.maxDamageValue);
						result.Add(statEntry);
						break;
					case(2):
						statEntry = _allStatsList.Where(s => s.Name == "Strength").FirstOrDefault() as Strength;
						statEntry.ConvertToIsOnItem(item.strengthValue);
						result.Add(statEntry);
						break; 
					case(3):
						statEntry = _allStatsList.Where(s => s.Name == "Dexterity").FirstOrDefault() as Dexterity;
						statEntry.ConvertToIsOnItem(item.dexterityValue);
						result.Add(statEntry);
						break;
					case(4):
						statEntry = _allStatsList.Where(s => s.Name == "Instinct").FirstOrDefault() as Instinct;
						statEntry.ConvertToIsOnItem(item.instinctValue);	
						result.Add(statEntry);
						break;
					case(5):				
						statEntry = _allStatsList.Where(s => s.Name == "Stamina").FirstOrDefault() as Stamina;
						statEntry.ConvertToIsOnItem(item.staminaValue);		
						result.Add(statEntry);
						break;
					case(6):
						statEntry = _allStatsList.Where(s => s.Name == "Critical Hit Chance").FirstOrDefault() as CriticalHitChance;
						statEntry.ConvertToIsOnItem(item.criticalHitChanceValue);			
						result.Add(statEntry);
						statEntry = null;
						break;
					case(7):
						statEntry = _allStatsList.Where(s => s.Name == "Critical Damage").FirstOrDefault() as CriticalDamage;
						statEntry.ConvertToIsOnItem(item.criticalDamageValue);	
						result.Add(statEntry);
						statEntry = null;
						break;
					case(8):
						statEntry = _allStatsList.Where(s => s.Name == "Dodge Chance").FirstOrDefault() as DodgeChance;
						statEntry.ConvertToIsOnItem(item.dodgeChanceValue);	
						result.Add(statEntry);
						statEntry = null;
						break;
					case(9):
						statEntry = _allStatsList.Where(s => s.Name == "Counter-Attack Chance").FirstOrDefault() as CounterAttackChance;
						statEntry.ConvertToIsOnItem(item.counterAttackChanceValue);
						result.Add(statEntry);
						statEntry = null;
						break;
					case(10):
						statEntry = _allStatsList.Where(s => s.Name == "Health Regen").FirstOrDefault() as HealthRegen;
						statEntry.ConvertToIsOnItem(item.healthRegenValue);				
						result.Add(statEntry);
						statEntry = null;
						break;
					default: 
						Log.DebugFormat("BuildStatsList, no case for Switch (stat.Key)");
						break;
					}
				}
			//Log.DebugFormat("created Added Stat list for an item. count: {0}", result.Count);
			return result;
		}

		private StatHolder SetupItemStatCollection(IEnumerable<IStat> stats)
		{
			return new StatHolder(stats);
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