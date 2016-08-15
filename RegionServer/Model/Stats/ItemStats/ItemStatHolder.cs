using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Logging;
using RegionServer.Model.Interfaces;

namespace RegionServer.Model.Stats.ItemStats
{
    public class ItemStatHolder : IStatHolder, IItemStatHolder
    {
        public ICharacter Character { get; set; }
        private Dictionary<Type, IStat> _stats;
        CCharacter IStatHolder.Character { get; set; }
        public Dictionary<Type, IStat> Stats { get { return _stats; } }
        protected static ILogger Log = LogManager.GetCurrentClassLogger();

        public ItemStatHolder(IEnumerable<IStat> stats)
        {
            _stats = new Dictionary<Type, IStat>();
            foreach (var stat in stats)
            {
                var derived = stat as IDerivedStat;
                if (derived != null)
                {
                    stat.IsOnItem = true;
                    stat.BaseValue = 0;
                    _stats.Add(stat.GetType(), stat);
                }
            }
        }

        public Dictionary<string, float> GetNonNullStats()
        {
            return _stats.ToDictionary(k => k.Value.Name, v => GetStatBase(v.Value));
        }

        public float GetStatBase<T>(T stat) where T : class, IStat
        {
            IStat result;
            _stats.TryGetValue(stat.GetType(), out result);
            if (result != null)
            {
                return result.BaseValue;
            }
            return 0;
        }

        public void SetStat<T>(float value) where T : class, IStat
        {
            IStat result;
            _stats.TryGetValue(typeof(T), out result);
            if (result != null)
            {
                result.BaseValue = (int)value;
            }
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

        public void SetStat<T>(T stat, float value) where T : class, IStat
        {
            //Log.DebugFormat("stat in holder num: {0}", _stats.Count);
            //_stats.ForEach(x => Log.DebugFormat("stats types:" + x.Value.GetType()));
            //Log.DebugFormat("INC TYPE: " + stat.GetType());
            IStat result;
            _stats.TryGetValue(stat.GetType(), out result);
            //Log.DebugFormat("setting item stat. value={0}, got stat out?: {1}", value, (result != null) ? true : false);
            if (result != null)
            {
                result.BaseValue = (int)value;
                //Log.DebugFormat("{0}, set stat: {1} - {2}", _stats.Count, result.Name, result.BaseValue);
            }

        }

        #region UNIMPLEMENTED
        public int ApplyDamage(int damage, CCharacter attacker)
        {
            return 0;
        }

        public int ApplyHeal(int healAmount)
        {
            return 0;
        }
        public float GetStat<T>(T stat) where T : class, IStat
        {
            return 0;
        }

        public float GetStat<T, C>(T stat, C target) where T : class, IStat where C : ICharacter
        {
            return 0;
        }

        public float GetStat<T>() where T : class, IStat
        {
            return 0;
        }

        public int ApplyDamage(int damage)
        {
            return 0;
        }

        public int HP5Regen()
        {
            return 0;
        }

        public bool Dirty { get; set; }
        public Dictionary<string, float> GetAllStats()
        {
            return null;
        }

        public Dictionary<string, float> GetMainStatsForEnemy()
        {
            return null;
        }

        public Dictionary<string, float> GetHealthLevel()
        {
            return null;
        }

        public void RefreshCurrentHealth()
        {
        }

        public float CalcStat(IStat stat)
        {
            throw new NotImplementedException();
        }

        public string SerializeStats()
        {
            return null;
        }

        public void DeserializeStats(string stats)
        {
        }
        #endregion

    }
}
