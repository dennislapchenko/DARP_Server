using System;
using RegionServer.Model.Interfaces;
using System.Collections.Generic;
using RegionServer.Calculators.Functions;
using RegionServer.Calculators.Lambdas;
using RegionServer.Model.Stats.PrimaryStats;

namespace RegionServer.Model.Stats
{
    public class MaxDamage : IDerivedStat
	{
		public string Name { get { return "Max Damage"; } }
        public int StatId { get; }
        public bool IsForCombat { get { return true;} }
		public bool IsBaseStat { get { return false;} }
		public bool IsNonNegative { get {return false; } }
		public bool IsNonZero { get { return true;} }
		public bool IsItemStat { get { return true; } }
		public float BaseValue { get { return _baseValue;} set{ _baseValue = value;} }
		public bool IsOnItem {get; set;}
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

		public MaxDamage()
		{
			_functions = new List<IFunction>() 
			{
				// strength base + (char base + char items) * scaler; 
				// 25 + (inst + items) * 0.09f;
				new FunctionAdd(this, 0, null, new LambdaStat(new Strength())),
				new FunctionMultiply(this, 1, null, new LambdaConstant(0.09f)),
				new FunctionAdd(this, 2, null, new LambdaConstant(19f)),
				new FunctionAdd(this, 0, null, new LambdaEquipment(this)),
                new FunctionAdd(this, 4, null, new LambdaEffect(this)),
            };

		}
	}
}
