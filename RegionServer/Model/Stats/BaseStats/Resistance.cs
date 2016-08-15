using System.Collections.Generic;
using RegionServer.Calculators.Functions;
using RegionServer.Calculators.Lambdas;
using RegionServer.Model.Interfaces;

namespace RegionServer.Model.Stats.BaseStats
{
    public class Resistance : IDerivedStat
    {
        public string Name { get {return "Resistance";} }
        public int StatId { get; }
        public bool IsBaseStat { get { return false; } }
        public bool IsNonZero { get { return false;} }
        public bool IsNonNegative { get { return true; }}
        public bool IsForCombat { get { return true;} }
        public bool IsOnItem { get; set; }
        public float BaseValue { get { return 25; } set { } }


        public void ConvertToIsOnItem(float value)
        {
        }

        private List<IFunction> _functions;
        public List<IFunction> Functions { get { return _functions; } }

        public Resistance()
        {
            _functions = new List<IFunction>()
            {
                new FunctionAdd(this, 0, null, new LambdaEquipment(this))
            };
        }
    }
}
