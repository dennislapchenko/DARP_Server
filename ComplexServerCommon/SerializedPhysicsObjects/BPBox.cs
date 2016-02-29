using System;
using ComplexServerCommon.MessageObjects;


namespace ComplexServerCommon.SerializedPhysicsObjects
{
	[Serializable]
	public class BPBox
	{
		public PositionData Center {get; set;}
		public PositionData HalfExtents {get; set;}
		public PositionData Rotation {get; set;}
		public PositionData LocalScale {get; set;}

		public BPBox()
		{
			Center = new PositionData();
			HalfExtents = new PositionData();
			Rotation = new PositionData();
			LocalScale = new PositionData();
		}

	}
}

