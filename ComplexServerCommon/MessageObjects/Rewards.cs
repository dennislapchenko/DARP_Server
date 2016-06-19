using System;
using ComplexServerCommon.Enums;

namespace ComplexServerCommon.MessageObjects
{
	[Serializable]
	public class Rewards
	{
		public int TotalDamage {get;set;}
		public int Experience {get;set;}
		public int Gold {get;set;}
		public int Skulls {get;set;}
		public ItemData Item {get;set;} //possibly a list
		public FightWinLossTie WLT {get;set;}
		public bool Injury {get;set;} //best reward ever <3
	}
}

