using System;
using ComplexServerCommon.MessageObjects;
using BEPUutilities;


namespace RegionServer.Model.Interfaces
{
	public interface IPhysics
	{
		Position Position {get; set;}
		bool Dirty {get; set;} //changed or not (should update or not)
		PlayerMovement Movement {get;set;}
		MoveDirection Direction {get; set;}
		float MoveSpeed {get; set;}
		Vector3 WalkDirection {get; set;}
		bool Moving {get; set;}
	}
}

