using System;
using ComplexServerCommon.MessageObjects.Enums;
using System.Collections.Generic;

namespace ComplexServerCommon.MessageObjects
{
	[Serializable]
	public struct ExchangeProfile
	{
		public int objectId {get;set;}
		public MoveOutcome outcome {get;set;}
		public int damage {get;set;}
		public int totalDamage {get;set;}

		public ExchangeProfile(int objId, int dmg, int totaldmg)
		{
			objectId = objId;
			outcome = MoveOutcome.Hit;
			damage = dmg;
			totalDamage = totaldmg;
		}
	}
}

