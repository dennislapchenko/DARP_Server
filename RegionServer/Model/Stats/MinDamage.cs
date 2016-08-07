using System;
using RegionServer.Model.Interfaces;
using System.Collections.Generic;
using RegionServer.Calculators.Functions;
using RegionServer.Calculators.Lambdas;
using RegionServer.Model.Stats.PrimaryStats;

namespace RegionServer.Model.Stats
{
    public class MinDamage : IDerivedStat
	{
		public string Name { get { return "Min Damage"; } }
        public int StatId { get; }
        public bool IsForCombat { get { return true;} }
		public bool IsBaseStat { get { return false;} }
		public bool IsNonNegative { get {return false; } }
		public bool IsNonZero { get { return true;} }
		public bool IsItemStat { get { return true; } }
		public bool IsOnItem {get; set;}
		public float BaseValue { get { return _baseValue;} set{ _baseValue = value;} }
		private float _baseValue = 0;

		private List<IFunction> _functions;
		public List<IFunction> Functions { get { return _functions; } }


		public void ConvertToIsOnItem(float value)
		{
			IsOnItem = true;
			_functions = new List<IFunction>()
			{
				new FunctionAdd(this, 0, null, new LambdaConstant(value)),
			};
		}

		public MinDamage()
		{
			_functions = new List<IFunction>() 
			{
				// strength base + (char base + char items) * scaler; 
				// 10 + (inst + items) * 0.09f;
				new FunctionAdd(this, 0, null, new LambdaStat(new Strength())),
				new FunctionMultiply(this, 1, null, new LambdaConstant(0.09f)),
				new FunctionAdd(this, 2, null, new LambdaConstant(10f)),
                new FunctionAdd(this, 3, null, new LambdaEffect(this)),
				new FunctionAdd(this, 4, null, new LambdaEquipment(this)),
            };
		}
	}
}

