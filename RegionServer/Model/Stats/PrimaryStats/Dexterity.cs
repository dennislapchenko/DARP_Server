using System;
using RegionServer.Model.Interfaces;
using System.Collections.Generic;
using RegionServer.Calculators.Functions;
using RegionServer.Calculators.Lambdas;


namespace RegionServer.Model.Stats.PrimaryStats
{
    [Serializable]
    //Buffs CRITICAL HIT CHANCE, CRITICAL DAMAGE, CRITICAL ANTI CHANCE & HIT CHANCE
    public class Dexterity : IDerivedStat
	{
		public string Name { get { return "Dexterity"; } }
        public int StatId { get { return 1;} }

        public bool IsNonNegative { get { return true; } } 
		public bool IsForCombat { get { return true; } }
		public bool IsBaseStat { get { return true; } }
		public bool IsNonZero { get { return false;} }
		public bool IsItemStat { get { return true;} }
		public bool IsOnItem {get; set;}
		public float BaseValue { get { return _baseValue;} set{ _baseValue = value;} }
		private float _baseValue = 5;

		private List<IFunction> _functions;
		public List<IFunction> Functions { get { return _functions; } }

		public void ConvertToIsOnItem(float value)
		{
			_functions = new List<IFunction>()
			{
				new FunctionAdd(this, 0, null, new LambdaConstant(value)),
			};
		}

		public Dexterity()
		{
			_functions = new List<IFunction>() 
			{
				new FunctionAdd(this, 0, null, new LambdaEquipment(this)),
                new FunctionAdd(this, 1, null, new LambdaEffect(this))
				//new FunctionSubtract(this, 1, null, new LambdaStat(new CritAChance(), true))
			};

		}
	}
}