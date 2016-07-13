using RegionServer.Model.Interfaces;
using System.Collections.Generic;
using System;
using ExitGames.Logging;
using RegionServer.Calculators;
using ComplexServerCommon;
using System.Linq;
using Environment = RegionServer.Calculators.Environment;
using ComplexServerCommon.MessageObjects;
using ComplexServerCommon.Enums;

namespace RegionServer.Model.Stats
{
	public class StatHolder : IStatHolder
	{
		public ICharacter Character {get; set;}
		private Dictionary<Type, IStat> _stats;
		public Dictionary<Type, IStat> Stats {get { return _stats; } }
		protected Dictionary<IStat, Calculator> Calculators = new Dictionary<IStat, Calculator>();
		private bool _dirty;

		protected ILogger Log = LogManager.GetCurrentClassLogger();

		public StatHolder(IEnumerable<IStat> stats)
		{
			_stats = new Dictionary<Type, IStat>();
			foreach(var stat in stats)
			{
				Calculators.Add(stat, new Calculator());
				_stats.Add(stat.GetType(), stat);
				var derived = stat as IDerivedStat;
				if (derived != null)
				{
					Calculators[stat].AddFunction(derived.Functions);
				}
			}
		}

		public bool Dirty
		{
			get 
			{
				if(GetStat<CurrHealth>() < GetStat<MaxHealth>())
				{
					if(Character.CurrentFight != null)
					{
						if(Character.CurrentFight.fightState != FightState.ENGAGED)
						{
							_dirty = true;
							return _dirty;
						}
						else
						{
							_dirty = false;
							return _dirty;
						}
					}
					_dirty = true;
				}
				return _dirty;
				
			}
			set { _dirty = value;}
		}

		public float GetStat<T>() where T : class, IStat
		{
			IStat result;
			_stats.TryGetValue(typeof (T), out result);
			if(result != null)
			{
				return CalcStat(result);
			}
			return 0;
		}

		public float GetStat<T>(T stat) where T : class, IStat
		{
			IStat result;
			_stats.TryGetValue(typeof(T), out result);
			if(result == null)
			{
				_stats.TryGetValue(((dynamic)stat).GetType(), out result);
			}
			if(result != null)
			{
				return CalcStat(result);
			}
			return 0;
		}

		public float GetStat<T, C>(T stat, C target)	where T : class, IStat 
														where C : ICharacter
		{
			IStat result;
			_stats.TryGetValue(typeof(T), out result);
			if(result == null)
			{
				_stats.TryGetValue(((dynamic)stat).GetType(), out result);
			}
			if(result != null && target != null)
			{
				return CalcStat(result, target);
			}
			return 0;
		}

		public float GetStatBase<T>() where T :class, IStat
		{
			IStat result;
			_stats.TryGetValue(typeof(T), out result);
			if(result != null)
			{
				return result.BaseValue;
			}
			return 0;
		}

	    public void AddToStat<T>(float value) where T : class, IStat
	    {
            IStat result;
            _stats.TryGetValue(typeof(T), out result);
            if (result != null)
            {
                result.BaseValue += (int)value;
            }
        }

	    public int ApplyDamage(int damage)
		{
			IStat health;
			_stats.TryGetValue(typeof(CurrHealth), out health);
			int currentHealth = (int)health.BaseValue;
			if(health != null)
			{
				//Log.DebugFormat("currHp: {0}, newHP: {1}, dmg: {2}, killdmg: {3}, overkill: {4}", currentHealth, -99, damage, -99, -99);
				var newHealth = currentHealth - damage;
				if(newHealth <= 0)
				{
					health.BaseValue = 0; //Dead
					Character.Die();
					//Log.DebugFormat("currHp: {0}, newHP: {1}, dmg: {2}, killdmg: {3}, overkill: {4}", currentHealth, newHealth, damage, -99, -99);
					var killDamage = damage - Math.Abs(newHealth);
					//Log.DebugFormat("currHp: {0}, newHP: {1}, dmg: {2}, killdmg: {3}, overkill: {4}", currentHealth, newHealth, damage, killDamage, -99);
					var overkill = damage - Math.Abs(currentHealth);
					//Log.DebugFormat("currHp: {0}, newHP: {1}, dmg: {2}, killdmg: {3}, overkill: {4}", currentHealth, newHealth, damage, killDamage, overkill);
					Log.DebugFormat("OVER-KILL. DMG TOTAL: {0} . ACTUAL DEALT: {1}({2})", damage, killDamage, overkill);
					return killDamage;
				}
				else
				{
					health.BaseValue = newHealth;
					return damage;
				}
			}
			return -9999999; //invalid result
		}

		public void RegenHealth()
		{
			SetStat<CurrHealth>(GetStat<CurrHealth>() + GetStat<HealthRegen>());
		}

		public void SetStat<T>(float value) where T : class, IStat
		{
			IStat result;
			_stats.TryGetValue(typeof(T), out result);
			if (result != null)
			{
				if(result.GetType() == typeof(CurrHealth) && value+GetStat<CurrHealth>() > GetStat<MaxHealth>())
				{
					result.BaseValue = GetStat<MaxHealth>();
				}
				else
				{
					result.BaseValue = value;
				}
			}
		}

	    public void SetStatByID(int id, float value)
	    {
	        IStat result = _stats.Values.FirstOrDefault(x => x.StatId == id);

	        if (result != null)
	        {
	            result.BaseValue += value;
	        }
	    }

		public float CalcStat(IStat stat)
		{
			return CalcStat(stat, null);
		}

		public float CalcStat(IStat stat, ICharacter target)
		{
			float returnValue = stat.BaseValue;

			var calculator = Calculators[stat];
			var env = new Environment() { Value = returnValue, Character = Character, Target = target};

			calculator.Calculate(env);

			if (env.Value <= 0 && stat.IsNonZero && !stat.IsNonNegative)
			{
				return 1;
			}
			if(env.Value <= 0 && stat.IsNonNegative)
			{
				return 0;
			}
			return env.Value;
		}

		public Dictionary<string, float> GetAllStats()
		{
			return _stats.ToDictionary(k => k.Value.Name, k => GetStat(k.Value));
        }

		public Dictionary<string, float> GetMainStatsForEnemy()
		{
		    var result = new Dictionary<string, float>()
		                                {
		                                    {_stats[typeof (Level)].Name, CalcStat(_stats[typeof (Level)])},
                                            {_stats[typeof (CurrHealth)].Name, CalcStat(_stats[typeof (CurrHealth)])},
                                            {_stats[typeof (MaxHealth)].Name, CalcStat(_stats[typeof (MaxHealth)])},
                                            {_stats[typeof (Strength)].Name, CalcStat(_stats[typeof (Strength)])},
                                            {_stats[typeof (Dexterity)].Name, CalcStat(_stats[typeof (Dexterity)])},
                                            {_stats[typeof (Instinct)].Name, CalcStat(_stats[typeof (Instinct)])},
                                            {_stats[typeof (Stamina)].Name, CalcStat(_stats[typeof (Stamina)])},
                                        };
			return result;
		}

		public Dictionary<string, float> GetHealthLevel()
		{
			var result = new Dictionary <string, float>()
		                                {
                                            { _stats[typeof(Level)].Name, CalcStat(_stats[typeof(Level)])},
                                            { _stats[typeof(CurrHealth)].Name, CalcStat(_stats[typeof(CurrHealth)])},
                                            { _stats[typeof(MaxHealth)].Name, CalcStat(_stats[typeof(MaxHealth)])}
		                                };
			return result;
		}

		public Dictionary<string, float> GetCurrMaxHealth()
		{
			return new Dictionary<string, float>()
			{
				{_stats[typeof(CurrHealth)].Name, CalcStat(_stats[typeof(CurrHealth)])},
				{_stats[typeof(MaxHealth)].Name, CalcStat(_stats[typeof(MaxHealth)])},
			};
		}

		public void RefreshCurrentHealth()
		{
			if(GetStat<CurrHealth>() > GetStat<MaxHealth>())
			{
				SetStat<CurrHealth>(GetStat<MaxHealth>());
			}
		}

		[Serializable]
		public class SerializedStat
		{
			public string StatType {get; set;}
			public float StatValue {get; set;}
		}

		public string SerializeStats()
		{
			List<SerializedStat> StatValues = new List<SerializedStat>();
			foreach(var stat in Stats.Values.ToList())
			{
				StatValues.Add(new SerializedStat(){StatType = stat.Name, StatValue = stat.BaseValue});
			}

			return SerializeUtil.Serialize(StatValues);
		}

		public void DeserializeStats(string stats)
		{
			foreach (var stat in SerializeUtil.Deserialize<List<SerializedStat>>(stats))
			{
				var result = _stats.Values.FirstOrDefault(s => s.Name == stat.StatType);
				if(result != null)
				{
					result.BaseValue = stat.StatValue;
				}
			}
		}
	}
}
