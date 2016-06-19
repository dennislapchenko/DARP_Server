using RegionServer.Model.Interfaces;


namespace RegionServer.Calculators.Lambdas
{
	public class LambdaConstant : ILambda
	{
		private readonly float _value;

		public LambdaConstant(float value)
		{
			_value = value;
		}

		#region ILambda implementation
		public float Calculate(Environment env)
		{
			return _value;
		}
		#endregion
	}
}

