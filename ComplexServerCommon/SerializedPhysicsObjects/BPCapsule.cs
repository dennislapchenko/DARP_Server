using System;
using BEPUutilities;


namespace ComplexServerCommon.SerializedPhysicsObjects
{
	[Serializable]
	public class BPCapsule
	{
		public Vector3 Center {get; set;}
		public float Height {get; set;}
		public float Radius {get; set;}
		public Vector3 Rotation {get; set;}
		public Vector3 LocalScale {get; set;}

		public BPCapsule()
		{
			Center = new Vector3();
			Rotation = new Vector3();
			LocalScale = new Vector3();
		}
	}
}

