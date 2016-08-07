using System;
using RegionServer.Model.Interfaces;

namespace RegionServer.Model.Stats.BaseStats
{
    public class Level : IStat
	{
		public Level()
		{
		}

		public bool IsOnItem { get; set; }

        public void ConvertToIsOnItem(float value)
		{
			throw new System.NotImplementedException();
		}

		public string Name {get { return "Level"; } }
        public int StatId { get; }

        public bool IsBaseStat { get { return true; } }
		public bool IsNonZero { get { return true; } }
		public bool IsNonNegative {get;}
		public bool IsForCombat {get;}

		public bool IsItemStat { get; }

        public float BaseValue { get; set; }
    }
}

