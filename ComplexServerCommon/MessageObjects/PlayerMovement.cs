using System;
namespace ComplexServerCommon.MessageObjects
{
	[Serializable]
	public class PlayerMovement
	{
		public int ObjectId {get; set;}
		public PositionData Position { get; set; }
	}
}