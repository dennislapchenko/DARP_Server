using System;
using RegionServer.Model.Interfaces;

namespace RegionServer.Model.Stats
{
    [Serializable]
    public class Level : IStat
	{
		public Level()
		{
		}

		public bool IsOnItem
		{
			get
			{
				throw new System.NotImplementedException();
			}
			set
			{
				throw new System.NotImplementedException();
			}
		}

		public void ConvertToIsOnItem(float value)
		{
			throw new System.NotImplementedException();
		}

		public string Name {get { return "Level"; } }

		public bool IsBaseStat { get { return true; } }
		public bool IsNonZero { get { return true; } }
		public bool IsNonNegative {get;}
		public bool IsForCombat {get;}

		public bool IsItemStat
		{
			get;
		}

		public float BaseValue { get; set; }
	}
}

