using System;
namespace ComplexServerCommon.MessageObjects
{
	[Serializable]
	public class PositionData
	{
		public float X {get; set;}
		public float Y {get; set;}
		public float Z {get; set;}
		
		public short Heading {get;set;} //0 - 65535 0.00549deg per value (to save message size)

		public PositionData()
			: this (0,0,0,0)
		{}

		public PositionData(float x, float y, float z)
			:this (x,y,z,0)
		{}

		public PositionData(float x, float y, float z, short heading)
		{
			X = x;
			Y = y;
			Z = z;
			Heading = heading;
		}

		public override string ToString()
		{
			return string.Format("X:{0}, Y:{1}, Z:{2}", X, Y, Z);
		}
	}
}

