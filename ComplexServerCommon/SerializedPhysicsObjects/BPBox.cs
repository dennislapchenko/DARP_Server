using System;
using BulletXNA.LinearMath;


namespace ComplexServerCommon.SerializedPhysicsObjects
{
	[Serializable]
	public class BPBox
	{
		public Vector3 Center {get; set;}
		public Vector3 HalfExtents {get; set;}

		public BPBox()
		{
			Center = new Vector3();
			HalfExtents = new Vector3();
		}

	}
}

