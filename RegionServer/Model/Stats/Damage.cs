using System;
using RegionServer.Model.Interfaces;
using System.Collections.Generic;
using RegionServer.Calculators.Functions;
using RegionServer.Calculators.Lambdas;

namespace RegionServer.Model.Stats
{
    public class Damage : IStat
	{
		public string Name { get { return "Damage"; } }
        public int StatId { get; }

        public void ConvertToIsOnItem(float value)
		{
			throw new NotImplementedException();
		}

		public bool IsNonNegative { get { return true;} } 
		public bool IsForCombat { get { return true;} }
		public bool IsBaseStat { get { return false;} }
		public bool IsNonZero { get { return false;} }
		public bool IsItemStat { get { return false; } }
		public float BaseValue { get { return 0;} set{} }
		public bool IsOnItem {get; set;}

		private List<IFunction> _functions;
		public List<IFunction> Functions { get { return _functions; } }

		public Damage(){}
	}
}
