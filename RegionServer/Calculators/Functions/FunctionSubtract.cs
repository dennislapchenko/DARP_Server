
using RegionServer.Model.Interfaces;
using RegionServer.Model;


namespace RegionServer.Calculators.Functions
{
	public class FunctionSubtract : IFunction
	{
		private readonly ILambda _lambda;
		
		#region IFunction implementation
		public IStat Stat {get;}
		public int Order {get;}
		public CObject Owner {get; set;}
		public ICondition Condition {get; set;}
		
		public void Calc(Environment env)
		{
			if(Condition == null || Condition.Test(env))
			{
				env.Value -= _lambda.Calculate(env);
			}
		}
		#endregion
		
		public FunctionSubtract(IStat stat, int order, CObject owner, ILambda lambda)
		{
			Stat = stat;
			Order = order;
			Owner = owner;
			_lambda = lambda;
		}
		
	}
}

