using System;
using System.Collections.Generic;

namespace ComplexServerCommon.MessageObjects
{
	public class CharFightInfo
	{
		public string Name {get;set;}
		public FightTeam Team {get;set;}
		public int ObjectId {get;set;}
		public List<KeyValuePairS<string, float>> stats;
		public List<KeyValuePairS<ItemSlot, ItemData>> equipment;
	}
}

