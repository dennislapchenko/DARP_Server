using System.Collections.Generic;

namespace ComplexServerCommon.MessageObjects
{
    [System.Serializable]
    public class UserFightInfo
    {
        public string Name;
        public int ObjectId;
        public int TargetId;
        public FightTeam Team;
		public Dictionary<string, float> stats;
		public Dictionary<ItemSlot, ItemData> equipment;
		//public List<KeyValuePairS<int, ItemData>> inventory;
	}
}

