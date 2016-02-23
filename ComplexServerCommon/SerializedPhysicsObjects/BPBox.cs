using System;
using BulletXNA.LinearMath;


namespace ComplexServerCommon.SerializedPhysicsObjects
{
	[Serializable]
	public class BPBox
	{
		public Vector3 Center {get; set;}
		public Vector3 HalfExtents {get; set;}
		public Vector3 Rotation {get; set;}
		public Vector3 LocalScale {get; set;}

		public BPBox()
		{
			Center = new Vector3();
			HalfExtents = new Vector3();
			Rotation = new Vector3();
			LocalScale = new Vector3();
		}

	}
}

