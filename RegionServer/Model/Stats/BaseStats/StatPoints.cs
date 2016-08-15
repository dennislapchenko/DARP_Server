using RegionServer.Model.Constants;
using RegionServer.Model.Interfaces;

namespace RegionServer.Model.Stats.BaseStats
{
    public class StatPoints : IStat
    {
        public string Name { get { return "StatPoints";  } }
        public int StatId { get; }
        public bool IsBaseStat { get { return false; } }
        public bool IsNonZero { get { return false;} }
        public bool IsNonNegative { get { return false; } }
        public bool IsForCombat { get { return false; } }
        public bool IsOnItem { get { return false; } set {} }
        public float BaseValue { get { return _baseValue; } set { _baseValue = value; } }

        private float _baseValue = RegionConstants.GetConstants(ConstantType.STAT_POINTS_PER_LEVEL)[0];

        public void ConvertToIsOnItem(float value)
        {
        }
    }
}
