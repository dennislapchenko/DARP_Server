using RegionServer.Calculators;

namespace RegionServer.Model.Interfaces
{
	public interface ICondition //listener 
	{
		bool Test(Environment env);
		void NotifyChanged(); //
	}
}

