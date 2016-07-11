using System;
using RegionServer.Model.Interfaces;
using System.Collections.Generic;
using RegionServer.Calculators.Functions;
using RegionServer.Calculators.Lambdas;


namespace RegionServer.Model.Stats
{
    [Serializable]
    public class HealthRegen : IDerivedStat
	{
		public string Name { get { return "Health Regen"; } } //hp5

		public bool IsNonNegative { get { return true;} } 
		public bool IsForCombat { get { return true;} }
		public bool IsBaseStat { get { return true;} }
		public bool IsNonZero { get { return false;} }
		public bool IsItemStat { get { return true; } }
		public bool IsOnItem {get; set;}
		public float BaseValue { get { return _baseValue;} set{_baseValue = value;} }
		private float _baseValue = 5;

		private List<IFunction> _functions;
		public List<IFunction> Functions { get { return _functions; } }

		public void ConvertToIsOnItem(float value)
		{
			IsOnItem = true;
			_baseValue = 0;
			_functions = new List<IFunction>()
			{
				new FunctionAdd(this, 0, null, new LambdaConstant(value)),
			};
		}

		public HealthRegen()
		{
			_functions = new List<IFunction>() 
			{
				// stamina base + (char base + char items) * scaler; 
				// 10 + (inst + items) * 0.05f;
				new FunctionAdd(this, 0, null, new LambdaStat(new Stamina())),
				new FunctionMultiply(this, 2, null, new LambdaConstant(0.08f)),
				new FunctionAdd(this, 1, null, new LambdaEquipment(this)),
				new FunctionAdd(this, 3, null, new LambdaConstant(10f)),
			};

		}
	}
}
