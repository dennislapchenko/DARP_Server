using System;
using System.Collections.Generic;
using ComplexServerCommon.Enums;

namespace ComplexServerCommon.MessageObjects
{
	[Serializable]
	public class FightQueueListItem
	{
		public string FightId {get;set;}
		public FightType Type {get; set;}
		public string Creator {get; set;} //to add creator straight into a team
		public List<string> Blue {get; set;}
		public List<string> Red {get; set;}
		public float Timeout {get; set;} //time after which the attack turn expires and is automatically calculated
		public int TeamSize {get; set;}
		public bool Sanguinary {get; set;} //can or cannot receive time-based, stat-lowering injury if defeated
	}
}

