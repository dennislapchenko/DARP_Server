using System.Collections.Generic;
using System;
using ComplexServerCommon.MessageObjects;

namespace RegionServer.Model.Interfaces
{
	public interface IStatHolder
	{
		CCharacter Character {get; set;}
		Dictionary<Type, IStat> Stats {get; }
		float GetStat<T>() where T : class, IStat;
		float GetStat<T>(T stat) where T : class, IStat;
		float GetStat<T, C>(T stat, C target) where T : class, IStat where C : ICharacter;
		float GetStatBase<T>(T stat) where T :class, IStat;
		void SetStat<T>(float value) where T : class, IStat;
	    void AddToStat<T>(float value) where T : class, IStat;
        int ApplyDamage(int damage, CCharacter attacker);
	    int ApplyHeal(int healAmount);
        int HP5Regen();
		bool Dirty {get;set;}
		//Dictionary<string, float> GetAllStats();
		Dictionary<string, float> GetAllStats();
        Dictionary<string, float> GetMainStatsForEnemy();
        Dictionary<string, float> GetHealthLevel();
		void RefreshCurrentHealth();
		float CalcStat(IStat stat);
		string SerializeStats();
		void DeserializeStats(string stats);

	}
}

