using System.Collections.Generic;

namespace RegionServer.Model.Interfaces
{
	public interface IInstance
	{
		int InstanceId {get; set;}
		Dictionary<byte, object> TeamA {get; set;}
		Dictionary<byte, object> TeamB {get; set;}
		Dictionary<byte, object> InstancePlayerStats {get; set;}



	}
}

