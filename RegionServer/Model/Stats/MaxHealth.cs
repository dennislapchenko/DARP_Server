using System;
using RegionServer.Model.Interfaces;
using System.Collections.Generic;
using RegionServer.Calculators.Functions;
using RegionServer.Calculators.Lambdas;


namespace RegionServer.Model.Stats
{
    [Serializable]
    public class MaxHealth : IDerivedStat
	{
		public string Name { get { return "MaxHealth"; } } //hp5

		public bool IsNonNegative { get { return true;} } 
		public bool IsForCombat { get { return true;} }
		public bool IsBaseStat { get { return false;} }
		public bool IsNonZero { get { return false;} }
		public bool IsItemStat { get { return true; } }
		public float BaseValue { get { return 0;} set{} }
		public bool IsOnItem {get; set;}

		private List<IFunction> _functions;
		public List<IFunction> Functions { get { return _functions; } }

		public void ConvertToIsOnItem(float value)
		{}


		public MaxHealth()
		{
			_functions = new List<IFunction>() 
			{
				// stamina base + (char base) * scaler; 
				// 1(stam) * 5f;
				new FunctionAdd(this, 0, null, new LambdaStat(new Stamina())),
				new FunctionMultiply(this, 2, null, new LambdaConstant(5)),
			};

		}
	}
}