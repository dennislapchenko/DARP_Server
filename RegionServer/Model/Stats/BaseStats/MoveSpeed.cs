
using System;
using RegionServer.Model.Interfaces;


namespace RegionServer.Model.Stats.BaseStats
{
    public class MoveSpeed : IStat
	{
		public MoveSpeed()
		{
		}

		public void ConvertToIsOnItem(float value)
		{
		}

		public bool IsOnItem { get; set; }

		public bool IsNonNegative
		{
			get { return true; }
		}

		public bool IsForCombat { get; }

		public bool IsItemStat { get; }

		#region IStat implementation
		public string Name { get { return "Move Speed"; } }
        public int StatId { get; }
        public bool IsBaseStat { get { return true; } }
		public bool IsNonZero { get { return false; } }
		public float BaseValue {get { return 0f; } set {} }
		#endregion
	}
}

