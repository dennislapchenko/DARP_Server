using System;
using RegionServer.Model.Interfaces;
using ComplexServerCommon.MessageObjects;
//using BulletXNA.LinearMath;
//using BulletXNA.BulletDynamics;

namespace RegionServer.Model
{
//	public class BulletPhysics : IPhysics
//	{
//		private bool _dirty;
//		private Position _position = new Position();
//		private PlayerMovement _playerMovement;
//		public ICharacterControllerInterface CharacterController {get; set;}
//		public Vector3 WalkDirection {get; set;}
//		public bool Moving {get; set;}
//		public float MoveSpeed {get; set;}
//
//		public Position Position {get { return _position;} set { _position = value; } }
//		public MoveDirection Direction {get; set;}
//
//		public bool Dirty
//		{
//			get
//			{
//				if (CharacterController != null && ((KinematicCharacterController)CharacterController).GetGhostObject() != null)
//				{
//					Position pos = ((KinematicCharacterController)CharacterController).GetGhostObject().GetWorldTransform();
//					if (_position != pos)
//					{
//						_position = pos;
//						_dirty = true;
//					}
//				}
//				return _dirty;
//			}
//			set
//			{
//				_dirty = value;
//			}
//		}
//
//		public PlayerMovement Movement {
//			get { return _playerMovement; } 
//			set 
//			{ 
//				_playerMovement = value;
//				if (CharacterController != null && ((KinematicCharacterController)CharacterController).GetGhostObject() != null)
//				{
//					Moving = false;
//					Direction = MoveDirection.None;
//					var xform = ((KinematicCharacterController)CharacterController).GetGhostObject().GetWorldTransform();
//					xform.SetRotation(Quaternion.CreateFromAxisAngle(new Vector3(0,1,0), Util.DegToRad((180 - (_playerMovement.Facing*0.0055f)))));
//					Vector3 forwardDir = xform._basis[2];
//					Vector3 upDir = xform._basis[1];
//					Vector3 strafeDir = xform._basis[0];
//					forwardDir.Normalize();
//					upDir.Normalize();
//					strafeDir.Normalize();
//
//					var moveSpeed = MoveSpeed;
//					WalkDirection = new Vector3(0,0,0);
//					if (_playerMovement.Walk)
//					{
//						moveSpeed /= 4.3f;
//					}
//
//					if(_playerMovement.Right < 0)
//					{
//						WalkDirection += strafeDir;
//						Direction |= MoveDirection.Right;
//						Moving = true;
//					}
//					if(_playerMovement.Right > 0)
//					{
//						WalkDirection -= strafeDir;
//						Direction |= MoveDirection.Left;
//						Moving = true;
//					}
//
//					if(_playerMovement.Forward < 0)
//					{
//						WalkDirection += forwardDir;
//						Direction |= MoveDirection.Forward;
//						Moving = true;
//					}
//					if(_playerMovement.Forward > 0)
//					{
//						WalkDirection -= forwardDir;
//						Direction |= MoveDirection.Backward;
//						Moving = true;
//					}
//
//					WalkDirection.Normalize();
//
//					Vector3 refVector = (WalkDirection * moveSpeed)/30f;
//					CharacterController.SetWalkDirection(ref refVector);
//
//				}
//			} 
//		}
//	}
}

