using System.Collections.Generic;

namespace ComplexServerCommon.MessageObjects
{
    [System.Serializable]
    public class ItemData
    {
        public string Name;
        public int ItemId;
        public int Type;
        public int Slot;
        public int Value;
        public int Equippable;
        public int LevelReq;
        public Dictionary<string, float> Stats;
		//public int Quality {get;set;}

		public ItemData()
		{
		}

		public ItemData(string name, int itemId, int type, int slot, int value, int equippable, int levelReq, Dictionary<string, float> stats)
		{
			Name = name;
			ItemId = itemId;
			Type = type;
			Slot = slot;
			Value = value;
			Equippable = equippable;
			LevelReq = levelReq;
			Stats = stats;
		}
	}
}

