using System;
using BulletXNA.LinearMath;


namespace ComplexServerCommon.SerializedPhysicsObjects
{
	[Serializable]
	public class BPCapsule
	{
		public Vector3 Center {get; set;}
		public float Height {get; set;}
		public float Radius {get; set;}

		public BPCapsule()
		{
			Center = new Vector3();
		}
	}
}

