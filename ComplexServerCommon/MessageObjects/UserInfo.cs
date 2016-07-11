using System.Collections.Generic;
using System.Xml.Serialization;

namespace ComplexServerCommon.MessageObjects
{
    [System.Serializable]
    public class UserInfo
	{
	    public PositionData Position;
        public string Name;
        public GenStatData GenStats;
        public int[] EquipmentKeys;
        public ItemData[] EquipmentValues;
        public int[] InventoryKeys;
        public ItemData[] InventoryValues;
        public Dictionary<string, float> Stats;
	}
}

