using RegionServer.Calculators;

namespace RegionServer.Model.Interfaces
{
	public interface IFunction //will modify stats, based on info given
	{
		IStat Stat {get;}
		int Order {get;} //figuring out when to multiply or when to add first
		CObject Owner {get; set;}
		ICondition Condition {get; set;}

		void Calc(Environment env);

	}
}

