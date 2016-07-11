using System;
using System.Collections.Generic;
using System.Linq;
using CloneExtensions;
using ComplexServerCommon.Enums;
using ComplexServerCommon.MessageObjects;
using ComplexServerCommon.MessageObjects.Enums;
using RegionServer.Model.Interfaces;
using RegionServer.Model.Items;
using RegionServer.Model.Stats;

namespace RegionServer.Model.Fighting
{
	public class FightUtils
	{
		private static List<string> blockOptions = new List<string>() {"01", "12", "23", "34", "40"};

		public static MoveOutcome CheckAhitB(FightMove A, FightMove B)
		{
			if (A.AttackSpot != B.BlockSpots[0] && A.AttackSpot != B.BlockSpots[1])
			{
				return MoveOutcome.Hit;
			}
			else
			{
				return MoveOutcome.Block;
			}
		}


		public static HitSpot getRandomHit()
		{
			return (HitSpot)RngUtil.intRange(0, 4);
		}

		public static List<HitSpot> getRandomBlock()
		{
			var option = blockOptions[RngUtil.intRange(0, 4)];
			return option.Select(c => (HitSpot) ((int) char.GetNumericValue(c))).ToList();
		}

		public static StatHolder getRandomStatsByLevel(IStatHolder statsToFill, byte level)
		{
			var statPointsPerLevel = 5;
			var statsToAccrue = level*statPointsPerLevel;

			while (statsToAccrue > 0)
			{
				switch (RngUtil.intRange(0, 3))
				{
					case (0):
						statsToFill.AddToStat<Strength>(1);
						break;
					case (1):
						statsToFill.AddToStat<Dexterity>(1);
						break;
					case (2):
						statsToFill.AddToStat<Instinct>(1);
						break;
					case (3):
						statsToFill.AddToStat<Stamina>(1);
						break;
				}
			    statsToAccrue--;
			}
            statsToFill.SetStat<CurrHealth>(statsToFill.GetStat<MaxHealth>());
			return (StatHolder)statsToFill;
		}

		public static ItemHolder getRandomItemsByLevel(IItemHolder itemsToFill, byte level)
		{
			//gets all items matching by level
			var itemsThatCanBeEquipped = ItemDBCache.Items.Values.Where(x => x.LevelReq == level || x.LevelReq == level-1);
			//loops through all item slots and picks a random item from the above variable, that match the slot
			foreach (var slot in Enum.GetValues(typeof(ItemSlot)).Cast<ItemSlot>())
			{
				var itemsMatchinTheSlot = itemsThatCanBeEquipped.Where(x => x.Slot == slot).ToList();
			    if (itemsMatchinTheSlot.Count == 0) continue;
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

	    public static StatHolder getStatHolder()
	    {
	        var stats = new List<IStat>
	                            {
	                                new CounterAttackChance(),
	                                new CriticalAntiHitChance(),
	                                new CriticalDamage(),
	                                new CriticalHitChance(),
	                                new CurrHealth(),
	                                new Damage(),
	                                new Dexterity(),
	                                new DodgeChance(),
	                                new HealthRegen(),
	                                new HitChance(),
	                                new Instinct(),
	                                new Dexterity(),
	                                new Strength(),
	                                new Stamina(),
	                                new Level(),
	                                new MaxDamage(),
	                                new MinDamage(),
	                                new ParryChance(),
	                                new MoveSpeed()
	                            };

	        return new StatHolder(stats);
	    }
	}
}
