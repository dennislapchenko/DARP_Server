using System;

namespace ComplexServerCommon.MessageObjects
{
	[Serializable]
	public class UserInfo
	{
		public PositionData Position {get; set;}
		public string Name {get; set;}

	}
}

