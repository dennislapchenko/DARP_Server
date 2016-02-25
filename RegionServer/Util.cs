using System;
using RegionServer.Model.Interfaces;


namespace RegionServer
{
	public class Util
	{
		public static bool IsInShortRange(int distanceToForgetObject, IObject obj1, IObject obj2, bool includeZAxis)
		{
			if(obj1 == null || obj2 == null)
			{
				return false;
			}

			if(distanceToForgetObject == -1)
			{
				return false;
			}

			float dx = obj1.Position.X - obj2.Position.X;
			float dy = obj1.Position.Y - obj2.Position.Y;
			float dz = obj1.Position.Z - obj2.Position.Z;

			return ((dx*dx) + (dy*dy) + (includeZAxis ? (dz*dz) : 0 )) <= distanceToForgetObject * distanceToForgetObject;
		}

		public static float DegToRad(float degrees)
		{
			return (float)Math.PI * (degrees/180f);
		}
	}
}

