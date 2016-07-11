using System;
using RegionServer.Model.Interfaces;
using System.Collections.Generic;
using RegionServer.Calculators.Functions;
using RegionServer.Calculators.Lambdas;


namespace RegionServer.Model.Stats
{
    [Serializable]
    public class CurrHealth : IStat
	{
		public string Name { get { return "CurrHealth"; } }

		public bool IsNonNegative { get { return true;} } 
		public bool IsForCombat { get { return true;} }
		public bool IsBaseStat { get { return false;} }
		public bool IsNonZero { get { return false;} }
		public bool IsItemStat { get { return false; } }
		public bool IsOnItem {get; set;}
		public float BaseValue { get { return _baseValue; } set { _baseValue = value; } }
		private float _baseValue = 0;


		public void ConvertToIsOnItem(float value)
		{
		}

		public CurrHealth()
		{

		}
	}
}