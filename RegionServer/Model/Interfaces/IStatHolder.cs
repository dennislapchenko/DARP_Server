using System.Collections.Generic;
using System;
using ComplexServerCommon.MessageObjects;

namespace RegionServer.Model.Interfaces
{
	public interface IStatHolder
	{
		ICharacter Character {get; set;}
		Dictionary<Type, IStat> Stats {get; }
		float GetStat<T>() where T : class, IStat;
		float GetStat<T>(T stat) where T : class, IStat;
		float GetStat<T, C>(T stat, C target) where T : class, IStat where C : ICharacter;
		float GetStatBase<T>() where T :class, IStat;
		void SetStat<T>(float value) where T : class, IStat;
		int ApplyDamage(int damage);
		void RegenHealth();
		bool Dirty {get;set;}
		//Dictionary<string, float> GetAllStats();
		List<KeyValuePairS<string, float>> GetAllStats();
		List<KeyValuePairS<string, float>> GetMainStatsForEnemy();
		List<KeyValuePairS<string, float>> GetHealthLevel();
		void RefreshCurrentHealth();
		float CalcStat(IStat stat);
		string SerializeStats();
		void DeserializeStats(string stats);

	}
}

