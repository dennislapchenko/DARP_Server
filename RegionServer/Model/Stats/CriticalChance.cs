
using RegionServer.Model.Interfaces;
using System.Collections.Generic;
using RegionServer.Calculators.Functions;
using RegionServer.Calculators.Lambdas;


namespace RegionServer.Model.Stats
{
	public class CriticalChance : IDerivedStat
	{
		public string Name { get { return "Critical hit chance"; } }
		public bool IsBaseStat { get { return false;} }
		public bool IsNonZero { get { return false;} }
		public float BaseValue { get { return 0;} set{} }
		
		private List<IFunction> _functions;
		public List<IFunction> Functions { get { return _functions; } }
		
		public CriticalChance()
		{
			_functions = new List<IFunction>() 
			{
				new FunctionAdd(this, 0, null, new LambdaConstant(0.1f)),
				new FunctionMultiply(this, 1, null, new LambdaStat(new Dexterity())) 
			};
			
		}
	}
}

