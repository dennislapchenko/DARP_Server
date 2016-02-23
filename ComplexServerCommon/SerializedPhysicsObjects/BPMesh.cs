using System;
using BulletXNA.LinearMath;
using System.Collections.Generic;

namespace ComplexServerCommon.SerializedPhysicsObjects
{
	[Serializable]
	public class BPMesh
	{
		public Vector3 Center {get; set;}

		public int NumTris {get; set;}
		public int NumVerts {get; set;}

		public List<int> Triangles {get; set;}
		public List<Vector3> Vertexes {get; set;}


		public BPMesh()
		{
			Center = new Vector3();
			Triangles = new List<int>();
			Vertexes = new List<Vector3>();
		}
	}
}

