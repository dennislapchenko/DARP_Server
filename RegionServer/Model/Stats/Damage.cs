using System.Collections.Generic;
using RegionServer.Calculators.Functions;
using RegionServer.Calculators.Lambdas;
using RegionServer.Model.Interfaces;
using RegionServer.Model.Stats.BaseStats;

namespace RegionServer.Model.Stats
{
    public class Damage : IDerivedStat
    {
        public string Name { get { return "Damage";} }
        public int StatId { get { return 1337; } }
        public bool IsBaseStat { get { return false;} }
        public bool IsNonZero { get { return false; } }
        public bool IsNonNegative { get { return false; } }
        public bool IsForCombat { get { return false; } }
        public bool IsOnItem { get; set; }
        public float BaseValue { get { return 0;} set {} }

        public void ConvertToIsOnItem(float value)
        {
        }

        private List<IFunction> _functions;
        public List<IFunction> Functions { get { return _functions; } }

        public Damage()
        {
            _functions = new List<IFunction>()
            {
                new FunctionAdd(this, 0, null, new LambdaStatMapToRange(new Resistance(), 10, 1000, 0.99f, 0.5f, true)), //true for - 'use target', aka gets targets stat
                new FunctionMultiply(this, 1, null, new LambdaRandomRange(new LambdaStat(new MinDamage()), new LambdaStat(new MaxDamage())))
            };
        }
    }
}
