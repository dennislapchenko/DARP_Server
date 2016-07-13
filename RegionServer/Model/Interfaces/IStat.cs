namespace RegionServer.Model.Interfaces
{
	public interface IStat
	{
		string Name {get;}
        int StatId { get; }

		bool IsBaseStat {get;}
		bool IsNonZero {get;}
		bool IsNonNegative {get;}
		bool IsForCombat {get;}
		bool IsOnItem {get; set;}

		float BaseValue {get; set;}

		void ConvertToIsOnItem(float value);
	}
}

