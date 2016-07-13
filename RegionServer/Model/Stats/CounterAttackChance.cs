using RegionServer.Model.Interfaces;
using System.Collections.Generic;
using RegionServer.Calculators.Functions;
using RegionServer.Calculators.Lambdas;

namespace RegionServer.Model.Stats
{
    public class CounterAttackChance : IDerivedStat
	{
		public string Name { get { return "Counter-Attack Chance"; } }
        public int StatId { get; }

        public bool IsNonNegative { get { return true;} } 
		public bool IsForCombat { get { return true;} }
		public bool IsBaseStat { get { return false;} }
		public bool IsNonZero { get { return false;} }
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

		public CounterAttackChance()
		{
			_functions = new List<IFunction>() 
			{
				// dex base + (char base + char items) * scaler; 
				// 30 + (inst + items) * 0.07f;
				new FunctionAdd(this, 0, null, new LambdaStat(new Instinct())),
				new FunctionAdd(this, 1, null, new LambdaEquipment(this)),
				new FunctionMultiply(this, 2, null, new LambdaConstant(0.07f)),
				new FunctionAdd(this, 3, null, new LambdaConstant(30f)),

			};

		}
	}
}

