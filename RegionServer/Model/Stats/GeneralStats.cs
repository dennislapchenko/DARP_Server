using System;
using ComplexServerCommon;
using ComplexServerCommon.MessageObjects;

namespace RegionServer.Model.Stats
{
	public class GeneralStats
	{
		public string Name {get;set;}
		public int Experience{get; set;}

		public int Battles{get;set;}
		public int Win {get; set;}
		public int Loss {get; set;}
		public int Tie {get; set;}

		public int Gold {get; set;}
		public int Skulls {get; set;}

		public int InventorySlots {get; set;}

		public GeneralStats()
		{
		}

		public static implicit operator GenStatData(GeneralStats gStats)
		{
			return new GenStatData(gStats.Name, gStats.Experience, gStats.Battles, gStats.Win, gStats.Loss, gStats.Tie, gStats.Gold, gStats.Skulls, gStats.InventorySlots);
		}

		public string SerializeStats()
		{
			return Xml.Serialize<GeneralStats>(this);
		}

		public void DeserializeStats(string genStats)
		{
			var gStats = Xml.Deserialize<GeneralStats>(genStats);
			this.Experience = gStats.Experience;
			this.Battles = gStats.Battles;
			this.Win = gStats.Win;
			this.Loss = gStats.Loss;
			this.Tie = gStats.Tie;
			this.Gold = gStats.Gold;
			this.Skulls = gStats.Skulls;
		}

		public void AddExperience(int value)
		{
			//add exp
			Experience += value;
			//check for level up
		}
	}
}

