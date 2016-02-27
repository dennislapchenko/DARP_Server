
using System.Collections.Generic;

namespace RegionServer.Model.Interfaces
{
	public interface IDerivedStat : IStat
	{
		List<IFunction> Functions {get; }
	}
}

