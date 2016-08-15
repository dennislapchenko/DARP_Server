using System.Collections.Generic;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;

namespace ComplexServerCommon.MessageObjects
{
    [System.Serializable]
    public class ItemData
    {
        public string Name;
        public string Description;
        public int ItemId;
        public int Type;
        public int Slot;
        public int Value;
        public int LevelReq;
        public byte MaxStack;
        public Dictionary<string, float> Stats;
        public string ConsumableEffect;
		//public int Quality {get;set;}

		public ItemData()
		{
		}

        public ItemData(string name, string descr, int itemId, int type, int slot, int value, byte maxstack, int levelReq, string effect, Dictionary<string, float> stats)
		{
			Name = name;
            Description = descr;
			ItemId = itemId;
			Type = type;
			Slot = slot;
			Value = value;
            MaxStack = maxstack;
			LevelReq = levelReq;
			Stats = stats;
		    ConsumableEffect = effect;
        }

        public ItemData(string name, string descr, int itemId, int type, int slot, int value, byte maxstack, int levelReq, string effect)
        :   this(name, descr, itemId, type, slot, value, maxstack, levelReq, effect, new Dictionary<string, float>())
        { 
        }
    }
}

