using System;
using System.Collections.Generic;

namespace ComplexServerCommon.MessageObjects
{
	[Serializable]
	public class UserFightInfo
	{
		public string Name {get;set;}
		public int ObjectId {get;set;}
		public int TargetId {get;set;}
		public FightTeam Team {get;set;}
		public List<KeyValuePairS<string, float>> stats;
		public List<KeyValuePairS<ItemSlot, ItemData>> equipment;
		//public List<KeyValuePairS<int, ItemData>> inventory;
	}
}

