using System;
using ComplexServerCommon.MessageObjects;

namespace ComplexServerCommon.SerializedPhysicsObjects
{
	[Serializable]
	public class BPTerrain
	{
		public PositionData Center {get; set;}
		public int Width {get; set;}
		public int Height {get; set;} //int because terrain uses height-fields (Bitmap heightmap image)

		public float[] HeightData {get; set;} //data 0.00 - 1.00
		public float HeightScale {get; set;} //0.00 - 600.00 scaling

		public PositionData LocalScale {get; set;}
		public PositionData Rotation {get; set;}


		public BPTerrain()
		{
			Center = new PositionData();
			LocalScale = new PositionData();
			Rotation = new PositionData();
		}
	}
}

