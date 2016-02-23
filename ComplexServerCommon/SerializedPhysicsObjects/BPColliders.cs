using System;
using System.Collections.Generic;

namespace ComplexServerCommon.SerializedPhysicsObjects
{
	[Serializable]
	public class BPColliders
	{
		public List<BPBox> Boxes {get; set;}
		public List<BPSphere> Spheres {get; set;}
		public List<BPCapsule> Capsules {get; set;}
		public List<BPTerrain> Terrains {get; set;}
		public List<BPMesh> Meshes {get; set;}

		public BPColliders()
		{
			Boxes = new List<BPBox>();
			Spheres = new List<BPSphere>();
			Capsules = new List<BPCapsule>();
			Terrains = new List<BPTerrain>();
			Meshes = new List<BPMesh>();
		}

	}
}

