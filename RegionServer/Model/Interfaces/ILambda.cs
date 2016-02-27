using RegionServer.Calculators;

namespace RegionServer.Model.Interfaces
{
	public interface ILambda //way of providing data to functions
	{
		float Calculate(Environment env);
	}
}

