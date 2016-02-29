using System;
using System.Collections.Generic;
using ComplexServerCommon.MessageObjects;

namespace ComplexServerCommon.SerializedPhysicsObjects
{
	[Serializable]
	public class BPMesh
	{
		public PositionData Center {get; set;}
		public PositionData Rotation {get; set;}
		public PositionData LocalScale {get; set;}

		public int NumTris {get; set;}
		public int NumVerts {get; set;}

		public List<int> Triangles {get; set;}
		public List<PositionData> Vertexes {get; set;}



		public BPMesh()
		{
			Rotation = new PositionData();
			LocalScale = new PositionData();

			Center = new PositionData();
			Triangles = new List<int>();
			Vertexes = new List<PositionData>();
		}
	}
}

