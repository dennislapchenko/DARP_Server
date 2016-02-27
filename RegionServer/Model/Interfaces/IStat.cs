namespace RegionServer.Model.Interfaces
{
	public interface IStat
	{
		string Name {get;}

		bool IsBaseStat {get;}
		bool IsNonZero {get;}

		float BaseValue {get; set;}
	}
}

