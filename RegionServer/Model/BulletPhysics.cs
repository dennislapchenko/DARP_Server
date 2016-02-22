using System;
using RegionServer.Model.Interfaces;
using ComplexServerCommon.MessageObjects;
using BulletXNA.LinearMath;
using BulletXNA.BulletDynamics;

namespace RegionServer.Model
{
	public class BulletPhysics : IPhysics
	{
		private bool _dirty;
		private Position _position = new Position();
		private PlayerMovement _playerMovement;
		public ICharacterControllerInterface CharacterController {get; set;}
		public Vector3 WalkDirection {get; set;}
		public bool Moving {get; set;}
		public float MoveSpeed {get; set;}

		public Position Position {get { return _position;} }

		public bool Dirty
		{
			get
			{
				if (CharacterController != null && ((KinematicCharacterController)CharacterController).GetGhostObject() != null)
				{
					Position pos = ((KinematicCharacterController)CharacterController).GetGhostObject().GetWorldTransform();
					if (_position != pos)
					{
						_position = pos;
						_dirty = true;
					}
				}
				return _dirty;
			}
			set
			{
				_dirty = value;
			}
		}

		public PlayerMovement Movement {
			get { return _playerMovement; } 
			set { _playerMovement = value; } 
		}
	}
}

