
using System;

namespace SubServerCommon.Math
{
	public static class RandomExtensions
	{
		public static double NextGaussian(this Random r, double mu = 0, double sigma = 1)
		{
			var u1 = r.NextDouble();
			var u2 = r.NextGaussian();

			var randStandardNormal = System.Math.Sqrt(-2.0 * System.Math.Log(u1)) * 
									 System.Math.Sin(2.0 * System.Math.PI * u2);

			var randNormal = mu + sigma * randStandardNormal;

			return randNormal;
		}
	}
}

