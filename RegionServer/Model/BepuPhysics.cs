using System;
using RegionServer.Model.Interfaces;
using ComplexServerCommon.MessageObjects;
using BEPUphysicsDemos.AlternateMovement.Character;
using BEPUutilities;

namespace RegionServer.Model
{
	public class BepuPhysics : IPhysics
	{
		private bool _dirty;
		private Position _position = new Position();
		private PlayerMovement _playerMovement;

		public CharacterController CharacterController {get; set;}
		public Vector3 WalkDirection {get; set;}
		public bool Moving {get; set;}
		public float MoveSpeed {get; set;}

		public Position Position {get { return _position;} set { _position = value; } }
		public MoveDirection Direction {get; set;}

		public bool Dirty
		{
			get
			{
				if (CharacterController != null && CharacterController.Body != null)
				{
					Position pos = CharacterController.Body.WorldTransform;
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
			set 
			{ 
				_playerMovement = value;
				if (CharacterController != null && CharacterController.Body != null)
				{
					Moving = false;
					Direction = MoveDirection.None;

					CharacterController.Body.Orientation = Quaternion.CreateFromAxisAngle(new Vector3(0,1,0), (float)-Math.PI*(_playerMovement.Facing*0.0055f)/180f);

					Vector3 forwardDir = CharacterController.Body.WorldTransform.Forward;
					Vector3 upDir = CharacterController.Body.WorldTransform.Up;
					Vector3 strafeDir = CharacterController.Body.WorldTransform.Right;
					forwardDir.Normalize();
					upDir.Normalize();
					strafeDir.Normalize();

					var moveSpeed = MoveSpeed;
					WalkDirection = new Vector3(0,0,0);
					if (_playerMovement.Walk)
					{
						moveSpeed /= 4.3f;
					}

					if(_playerMovement.Right < 0)
					{
						WalkDirection -= strafeDir;
						Direction |= MoveDirection.Right;
						Moving = true;
					}
					if(_playerMovement.Right > 0)
					{
						WalkDirection += strafeDir;
						Direction |= MoveDirection.Left;
						Moving = true;
					}

					if(_playerMovement.Forward < 0)
					{
						WalkDirection -= forwardDir;
						Direction |= MoveDirection.Forward;
						Moving = true;
					}
					if(_playerMovement.Forward > 0)
					{
						WalkDirection += forwardDir;
						Direction |= MoveDirection.Backward;
						Moving = true;
					}

					WalkDirection.Normalize();

					Vector3 refVector = (WalkDirection * moveSpeed);
					CharacterController.HorizontalMotionConstraint.MovementDirection = new Vector2(refVector.X, refVector.Z);

				}
			} 
		}
	}
}

