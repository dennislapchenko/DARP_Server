using System;
using BulletXNA.LinearMath;


namespace ComplexServerCommon.SerializedPhysicsObjects
{
	[Serializable]
	public class BPSphere
	{
		public Vector3 Center {get; set;}
		public float Radius {get; set;}
		public Vector3 Rotation {get; set;}
		public Vector3 LocalScale {get; set;}

		public BPSphere()
		{
			Center = new Vector3();
			Rotation = new Vector3();
			LocalScale = new Vector3();
		}
	}
}

