using System;
using ComplexServerCommon.MessageObjects.Enums;

namespace ComplexServerCommon.MessageObjects
{
	[Serializable]
	public class ExchangeProfile
	{
		public int objectId {get;set;}
		public MoveOutcome outcome {get;set;}
		public int damage {get;set;}
		public int totalDamage {get;set;}

		public ExchangeProfile()
		{
		}
	}
}

