using RegionServer.Model.Interfaces;

namespace RegionServer.Model.Stats
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

        private float _baseValue = 5;



        public void ConvertToIsOnItem(float value)
        {
            throw new System.NotImplementedException();
        }
    }
}
