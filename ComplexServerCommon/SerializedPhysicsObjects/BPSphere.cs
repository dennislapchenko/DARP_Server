using System;
using BulletXNA.LinearMath;


namespace ComplexServerCommon.SerializedPhysicsObjects
{
	[Serializable]
	public class BPSphere
	{
		public Vector3 Center {get; set;}
		public float Radius {get; set;}

		public BPSphere()
		{
			Center = new Vector3();
		}
	}
}

