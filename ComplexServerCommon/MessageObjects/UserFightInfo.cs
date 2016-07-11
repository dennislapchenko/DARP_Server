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
		public List<KeyValuePairS<ItemSlot, ItemData>> equipment;
		//public List<KeyValuePairS<int, ItemData>> inventory;
	}
}

