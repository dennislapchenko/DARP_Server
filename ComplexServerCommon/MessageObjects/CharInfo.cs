using System.Collections.Generic;


namespace ComplexServerCommon.MessageObjects
{
    [System.Serializable]
    public class CharInfo
	{
		//anything others need to know about character on update
	    public PositionData Position;
	    public int ObjectId;
	    public string Name;
	    public GenStatData GenStats;
	    public Dictionary<string, float> Stats;
        public Dictionary<int, ItemData> Equipment; 

		public CharInfo()
		{
			
		}
	}
}

