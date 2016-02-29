using System;
namespace ComplexServerCommon.MessageObjects
{
	[Serializable]
	public class MoveTo
	{
		public MoveTo()
		{
			CurrentPosition = new PositionData();
			Destination = new PositionData();
		}
		public PositionData CurrentPosition {get; set;}
		public PositionData Destination {get; set;}
		public int Facing {get; set;}
		public float Speed {get; set;}
		public bool Moving {get; set;}
		public MoveDirection Direction {get; set;}
	}
}

