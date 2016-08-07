using System;
using RegionServer.Model.Interfaces;
using System.Collections.Generic;
using RegionServer.Calculators.Functions;
using RegionServer.Calculators.Lambdas;


namespace RegionServer.Model.Stats.SecondaryStats
{
    public class HitChance : IDerivedStat
	{
		public string Name { get { return "Hit Chance"; } }
        public int StatId { get; }

        public bool IsNonNegative { get { return true;} } 
		public bool IsForCombat { get { return true;} }
		public bool IsBaseStat { get { return false;} }
		public bool IsNonZero { get { return false;} }
		public bool IsItemStat { get { return false; } }
		public bool IsOnItem {get; set;}

		public float BaseValue { get { return _baseValue;} set{_baseValue = value;} }
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

		public HitChance()
		{
			_functions = new List<IFunction>() 
			{
				// hit base + (char base + char items) * scaler; 
				// 70 + (dex + items) * 0.06f;
//				new FunctionAdd(this, 0, null, new LambdaStat(new Dexterity())),
//				new FunctionAdd(this, 1, null, new LambdaEquipment(this)),
//				new FunctionMultiply(this, 2, null, new LambdaConstant(0.06f)),
				new FunctionAdd(this, 3, null, new LambdaConstant(100f)),

				new FunctionSubtract(this, 4, null, new LambdaStat(new DodgeChance(), true)),
                new FunctionAdd(this, 5, null, new LambdaEffect(this))
				//new FunctionSubtract(this, 4, null, new LambdaStat(new WeaponSkill(), true))
			};

		}
	}
}
