using System.Collections.Generic;
using System;

namespace RegionServer.Model.Interfaces
{
	public interface IStatHolder
	{
		ICharacter Character {get; set;}
		Dictionary<Type, IStat> Stats {get; }
		float GetStat<T>() where T : class, IStat;
		float GetStat<T>(T stat) where T : class, IStat;
		void SetStat<T>(float value) where T : class, IStat;
		string SerializeStats();
		void DeserializeStats(string stats);

	}
}

