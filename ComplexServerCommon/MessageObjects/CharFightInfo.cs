using System.Collections.Generic;

namespace ComplexServerCommon.MessageObjects
{
    [System.Serializable]
    public class CharFightInfo
	{
	    public string Name;
	    public FightTeam Team;
	    public int ObjectId;
		public Dictionary<string, float> stats;
        public Dictionary<int, ItemData> equipment;
	}
}

