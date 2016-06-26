using System;
using System.Collections.Generic;
using System.Linq;
using ComplexServerCommon.Enums;
using ComplexServerCommon.MessageObjects;
using RegionServer.Model.Interfaces;
using RegionServer.Model.Items;
using RegionServer.Model.Stats;

namespace RegionServer
{
	public class FightUtils
	{
		private static List<string> blockOptions = new List<string>() {"01", "12", "23", "34", "40"}; 

		public static HitSpot getRandomHit()
		{
			return (HitSpot) new Random().Next(0, 5);
		}

		public static List<HitSpot> getRandomBlock()
		{
			var option = blockOptions[new Random().Next(0, 5)];
			return option.Select(c => (HitSpot) ((int) char.GetNumericValue(c))).ToList();
		}

		public static StatHolder getRandomStatsByLevel(IStatHolder statsToFill, byte level)
		{
			var statPointsPerLevel = 5;
			var statsToAccrue = level*statPointsPerLevel;

			while (statsToAccrue > 0)
			{
				switch (new Random().Next(0, 4))
				{
					case (0):
						statsToFill.SetStat<Strength>(statsToAccrue--);
						break;
					case (1):
						statsToFill.SetStat<Dexterity>(statsToAccrue--);
						break;
					case (2):
						statsToFill.SetStat<Instinct>(statsToAccrue--);
						break;
					case (3):
						statsToFill.SetStat<Stamina>(statsToAccrue--);
						break;
				}
			}
			return (StatHolder)statsToFill;
		}

		public static ItemHolder getRandomItemsByLevel(IItemHolder itemsToFill, byte level)
		{
			//gets all items matching by level
			var itemsThatCanBeEquipped = ItemDBCache.Items.Values.Where(x => x.LevelReq == level || x.LevelReq == level-2);
			//loops through all item slots and picks a random item from the above variable, that match the slot
			foreach (var slot in Enum.GetValues(typeof (ItemSlot)).Cast<ItemSlot>())
			{
				var itemsMatchinTheSlot = itemsThatCanBeEquipped.Where(x => x.Slot == slot).ToList();
				itemsToFill.EquipItemOnRestore(itemsMatchinTheSlot[new Random().Next(0, itemsMatchinTheSlot.Count)].ItemId);
			}
			return (ItemHolder)itemsToFill;
		}

		public static GeneralStats getRandomGenStats(GeneralStats genStats, byte level)
		{
			genStats.Battles = new Random().Next(level*5, level*15);
			genStats.Win = new Random().Next(level * 5, level * 15);
			genStats.Loss = new Random().Next(level * 5, level * 15);
			genStats.Tie = new Random().Next(level * 1, level * 5);

			return genStats;
		}
	}
}
