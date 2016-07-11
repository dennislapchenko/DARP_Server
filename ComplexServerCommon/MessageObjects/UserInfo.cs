using System.Collections.Generic;

namespace ComplexServerCommon.MessageObjects
{
    [System.Serializable]
    public class UserInfo
	{
	    public PositionData Position;
        public string Name;
        public GenStatData GenStats;
        public Dictionary<int, ItemData> Equipment;
        public Dictionary<int, ItemData> Inventory;
        public Dictionary<string, float> Stats;
	}
}

