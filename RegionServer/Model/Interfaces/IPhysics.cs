using System;
using ComplexServerCommon.MessageObjects;
using BulletXNA.LinearMath;


namespace RegionServer.Model.Interfaces
{
	public interface IPhysics
	{
		Position Position {get; }
		bool Dirty {get; set;} //changed or not (should update or not)
		PlayerMovement Movement {get;set;}
		float MoveSpeed {get; set;}
		Vector3 WalkDirection {get; set;}
		bool Moving {get; set;}
	}
}

