using System.Collections.Generic;

namespace ComplexServerCommon.MessageObjects
{
    [System.Serializable]
    public class TestDictObj
    {
        public Dictionary<ItemSlot, ItemData> Equipment;
        public Dictionary<int, ItemData> Inventory;
        public GenStatData GenStats;
        public Dictionary<string, float> Stats;
    }
}
