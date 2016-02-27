using RegionServer.Model.Interfaces;
using System.Collections.Generic;


namespace RegionServer.Calculators.Lambdas
{
	public class LambdaCalculator : ILambda
	{
		public List<IFunction> Functions { get; private set;}

		public LambdaCalculator()
		{
			Functions = new List<IFunction>();
		}

		public float Calculate(Environment env)
		{
			float saveValue = env.Value;
			float returnValue = 0;
			env.Value = 0;
			foreach (var function in Functions)
			{
				function.Calc(env);
			}
			returnValue = env.Value;
			env.Value = saveValue;
			return returnValue;
		}
	}
}

