// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
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

