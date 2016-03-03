using System.Collections.Generic;

namespace RegionServer.Model.Interfaces
{
	public interface IInstance
	{
		int InstanceId {get; set;}
		Dictionary<short, CPlayerInstance> TeamA {get; set;}
		Dictionary<short, CPlayerInstance> TeamB {get; set;}

		Dictionary<short, IMoveExchange> Moves {get; set;}




	}
}

