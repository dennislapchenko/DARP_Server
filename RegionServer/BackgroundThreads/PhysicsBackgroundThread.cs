
using MMO.Framework;
using BulletXNA.BulletCollision;
using RegionServer.Model;
using BulletXNA.BulletDynamics;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Threading;
using RegionServer.Model.Interfaces;
using BulletXNA.LinearMath;
using BulletXNA;

namespace RegionServer.BackgroundThreads
{
	public class PhysicsBackgroundThread : IBackgroundThread
	{
		//run roughly at 60fps
		public Region Region {get; set;}
		private bool isRunning;
		private DiscreteDynamicsWorld dynamicsWorld;
		private List<CollisionShape> collisionShapes;

		public float characterHeight = 1.75f;
		public float characterWidth = 0.75f;

		public PhysicsBackgroundThread(Region region, IEnumerable<IPlayerListener> playerListeners)
		{
			collisionShapes = new List<CollisionShape>();
			Region = region;
			Region.OnAddPlayer += OnAddPlayer;
			Region.OnRemovePlayer += OnRemovePlayer;
			foreach (var playerListener in playerListeners)
			{
				playerListener.OnAddPlayer += OnAddPlayer;
				playerListener.OnRemovePlayer += OnRemovePlayer;
			}
		}

		private void OnAddPlayer(IPlayer player)
		{
			var obj = player as IObject;
			Matrix startTransform = obj != null 
												? Matrix.CreateTranslation(obj.Position.X, obj.Position.Y, obj.Position.Z) 
												: Matrix.CreateTranslation(0,0,0);
			var ghostObject = new PairCachingGhostObject(); //generic collision shape (capsule)
			ghostObject.SetWorldTransform(startTransform);
			ConvexShape capsule = new CapsuleShape(characterWidth, characterHeight);
			ghostObject.CollisionShape = capsule;
			ghostObject.SetCollisionFlags(CollisionFlags.CharacterObject); //which objects can run past eachother (ignore collision)

			float stepHeight = 0.35f; //max step up a foot(else have to jump)
			var character = new KinematicCharacterController(ghostObject, capsule, stepHeight, 1); //1 is like unity, Y axis is up;

			dynamicsWorld.AddCollisionObject(ghostObject, CollisionFilterGroups.CharacterFilter, CollisionFilterGroups.StaticFilter|CollisionFilterGroups.DefaultFilter);
			dynamicsWorld.AddAction(character);

			((BulletPhysics)player.Physics).CharacterController = character;
		}

		private void OnRemovePlayer(IPlayer player)
		{
			lock (this)
			{
				dynamicsWorld.RemoveAction(((BulletPhysics)player.Physics).CharacterController);
				dynamicsWorld.RemoveCollisionObject(((KinematicCharacterController)((BulletPhysics)player.Physics).CharacterController).GetGhostObject());
			}
		}

		public void Setup()
		{
			DefaultCollisionConfiguration collisionConfiguration = new DefaultCollisionConfiguration();
			CollisionDispatcher dispatcher = new CollisionDispatcher(collisionConfiguration);
			Vector3 min = new Vector3(-1000, -1000, -1000), max = new Vector3(1000, 1000, 1000);
			var overlappingPairCache = new AxisSweep3Internal(ref min, ref max, 0xfff3, 0xffff, 16384, null, false); //16384 max objects in Region
			SequentialImpulseConstraintSolver solver = new SequentialImpulseConstraintSolver();

			dynamicsWorld = new DiscreteDynamicsWorld(dispatcher, overlappingPairCache, solver, collisionConfiguration);
			dynamicsWorld.DispatchInfo.SetAllowedCcdPenetration(0.0001f);//how far object pass through the world before its reset

			overlappingPairCache.GetOverlappingPairCache().SetInternalGhostPairCallback(new GhostPairCallback()); //when object collide they handle themselves
			dynamicsWorld.SetGravity(new Vector3(0, -10, 0)); //-10 metres per second (9.8 is earth)

			CollisionShape groundShape = new BoxShape(new Vector3(50,50,50)); //earth shape
			Matrix groundTransform = Matrix.CreateTranslation(0, -56, 0); //

			DefaultMotionState motionState = new DefaultMotionState(groundTransform, Matrix.Identity); //what does when in motion, where moved to, where center is
			RigidBodyConstructionInfo rbInfo = new RigidBodyConstructionInfo(0, motionState, groundShape, new Vector3(0,0,0));
			RigidBody body = new RigidBody(rbInfo);

			dynamicsWorld.AddRigidBody(body); //with automatic CollisionFilterGroups.DefaultFilter


		}

		public void Run(object threadContext)
		{
			Stopwatch timer = new Stopwatch();
			timer.Start();
			isRunning = true;

			while(isRunning)
			{
				if(timer.Elapsed < TimeSpan.FromSeconds(1/60f))
				{
					if(Region.NumPlayers <= 0)
					{
						Thread.Sleep(1000);
					}
					continue;
				}
				Update(timer.Elapsed);
				timer.Restart();
			}
		}

		private void Update(TimeSpan elapsed)
		{
			lock(this)
			{
				dynamicsWorld.StepSimulation(elapsed.Milliseconds/1000f, 1);
			}
		}

		public void Stop()
		{
			isRunning = false;
			for(int i = dynamicsWorld.NumCollisionObjects; i >= 0; i--)
			{
				CollisionObject obj = dynamicsWorld.CollisionObjectArray[i];
				dynamicsWorld.RemoveCollisionObject(obj);
			}
			collisionShapes.Clear();
		}
	
	}
}

