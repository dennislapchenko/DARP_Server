using System;
using BulletXNA.LinearMath;

namespace ComplexServerCommon.SerializedPhysicsObjects
{
	[Serializable]
	public class BPTerrain
	{
		public Vector3 Center {get; set;}
		public int Width {get; set;}
		public int Height {get; set;} //int because terrain uses height-fields (Bitmap heightmap image)

		public float[] HeightData {get; set;} //data 0.00 - 1.00
		public float HeightScale {get; set;} //0.00 - 600.00 scaling

		public Vector3 LocalScale {get; set;}
		public Vector3 Rotation {get; set;}


		public BPTerrain()
		{
			Center = new Vector3();
			LocalScale = new Vector3();
			Rotation = new Vector3();
		}
	}
}

