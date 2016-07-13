using System;
using RegionServer.Model.Interfaces;
using System.Collections.Generic;
using RegionServer.Calculators.Functions;
using RegionServer.Calculators.Lambdas;


namespace RegionServer.Model.Stats
{
    //Buffs DODGE, PARRY & COUNTER-ATTACK CHANCE
    public class Instinct : IDerivedStat
	{
		public string Name { get { return "Instinct"; } }
        public int StatId { get { return 2; } }

        public bool IsNonNegative { get { return true;} } 
		public bool IsForCombat { get { return true;} }
		public bool IsBaseStat { get { return true;} }
		public bool IsNonZero { get { return false;} }
		public bool IsOnItem {get; set;}
		public bool IsItemStat { get { return true;} }
		public float BaseValue { get { return _baseValue;} set{ _baseValue = value;} }
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

		public Instinct()
		{
			_functions = new List<IFunction>() 
			{
				new FunctionAdd(this, 1, null, new LambdaEquipment(this))
				//new FunctionSubtract(this, 1, null, new LambdaStat(new CritAChance(), true))
			};

		}
	}
}