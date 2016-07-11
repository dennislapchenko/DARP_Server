using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComplexServerCommon.MessageObjects
{
    public class TestDictObj
    {

        public Dictionary<ItemSlot, ItemData> Equipment { get; set; }
        public Dictionary<int, ItemData> Inventory { get; set; }
        public GenStatData GenStats { get; set; }
        public Dictionary<string, float> Stats { get; set; }
    }
}
