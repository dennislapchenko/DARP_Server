
using RegionServer.Model.Interfaces;

namespace RegionServer.Calculators.Lambdas
{
	public class LambdaStat : ILambda
	{
		private readonly IStat _stat;
		private readonly bool _useTarget;

		public LambdaStat(IStat stat, bool useTarget = false)
		{
			_stat = stat;
			_useTarget = useTarget;
		}

		#region ILambda implementation
		public float Calculate(Environment env)
		{
			if(_useTarget && env.Target == null)
			{
				return 1;
			}
			if(!_useTarget && env.Character == null)
			{
				return 1;
			}
			if(_useTarget)
			{
				return env.Target.Stats.GetStat(_stat);
			}

			return env.Character.Stats.GetStat(_stat);
		}
		#endregion
	}
}

