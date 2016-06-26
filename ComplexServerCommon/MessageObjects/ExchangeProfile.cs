using System;
using ComplexServerCommon.MessageObjects.Enums;

namespace ComplexServerCommon.MessageObjects
{
	[Serializable]
	public struct ExchangeProfile
	{
		public int objectId {get;set;}
		public MoveOutcome outcome {get;set;}
		public int damage {get;set;}
		public int totalDamage {get;set;}

		public ExchangeProfile(int objId)
		{
			objectId = objId;
			outcome = MoveOutcome.Hit;
			damage = 0;
			totalDamage = 0;
		}
	}
}

