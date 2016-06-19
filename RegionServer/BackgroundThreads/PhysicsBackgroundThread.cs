
using MMO.Framework;
using RegionServer.Model;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Threading;
using RegionServer.Model.Interfaces;
using System.IO;
using MMO.Photon.Application;
using SubServerCommon;
using SubServerCommon.Data.NHibernate;
using System.Linq;
using ComplexServerCommon.SerializedPhysicsObjects;
using ComplexServerCommon;
using System.Xml.Serialization;
using BEPUphysics;
using BEPUutilities.Threading;
using BEPUphysics.CollisionRuleManagement;
using BEPUutilities;
using BEPUphysicsDemos.AlternateMovement.Character;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.BroadPhaseEntries;

namespace RegionServer.BackgroundThreads
{
	public class PhysicsBackgroundThread : IBackgroundThread
	{
		private const float FPS = 30f;

		public Region Region {get; set;}
		private bool isRunning;
		protected PhotonApplication Server {get; set;}
		public Space Space {get; set;}
		private ParallelLooper parallelLooper;
		CollisionGroup characters = new CollisionGroup();

		public float characterHeight = 1.75f;
		public float characterWidth = 0.75f;

		public PhysicsBackgroundThread(Region region, IEnumerable<IPlayerListener> playerListeners, PhotonApplication application)
		{
			Server = application;
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

			var cc = ((BepuPhysics)player.Physics).CharacterController = new CharacterController (new Vector3(obj.Position.X, obj.Position.Y, obj.Position.Z), characterHeight, characterHeight/2f, characterWidth, 10);

			cc.Body.CollisionInformation.CollisionRules.Group = characters;

			Space.Add(cc);
		}

		private void OnRemovePlayer(IPlayer player)
		{
			lock (this)
			{
				Space.Remove(((BepuPhysics)player.Physics).CharacterController);
			}
		}

		public void Setup()
		{
			parallelLooper = new ParallelLooper();
			parallelLooper.AddThread();
			parallelLooper.AddThread();
			parallelLooper.AddThread();
			parallelLooper.AddThread();

			Space = new Space(parallelLooper);

			Space.ForceUpdater.Gravity = new Vector3(0, -10, 0);
			Space.TimeStepSettings.TimeStepDuration = 1f/FPS; //step calculation time. 1f/30f = 30fps

			var groupPair = new CollisionGroupPair(characters, characters);
			CollisionRules.CollisionGroupRules.Add(groupPair, CollisionRule.NoBroadPhase); //passes right through other characters


			string FilePath = Path.Combine(Server.BinaryPath, "default.xml");
			try
			{
				using(var session = NHibernateHelper.OpenSession())
				{
					using(var transaction = session.BeginTransaction())
					{
						var region = session.QueryOver<RegionRecord>().Where(rr => rr.Name == Server.ApplicationName).SingleOrDefault();
						if(region != null)
						{
							FilePath = Path.Combine(Server.BinaryPath, region.ColliderPath);
						}
					}
				}
			}
			finally {}

			XmlSerializer serializer = new XmlSerializer(typeof(BPColliders));
			FileStream f = File.OpenRead(FilePath);
			BPColliders colliders = (BPColliders)serializer.Deserialize(f);
			
			//Box Colliders
			foreach (var bpBox in colliders.Boxes)
			{
				var groundShape = new Box((Vector3)(Position)bpBox.Center, bpBox.LocalScale.X * bpBox.HalfExtents.X *2,
				                          bpBox.LocalScale.Y * bpBox.HalfExtents.Y *2,
				                          bpBox.LocalScale.Z * bpBox.HalfExtents.Z *2);
				groundShape.Orientation = new Quaternion(bpBox.Rotation.X, bpBox.Rotation.Y, bpBox.Rotation.Z, bpBox.Rotation.W);
				groundShape.IsAffectedByGravity = false;
				Space.Add(groundShape);
			}


			//Capsule Colliders
			foreach (var bpCapsule in colliders.Capsules)
			{
				var groundShape = new Capsule((Vector3)(Position)bpCapsule.Center, bpCapsule.LocalScale.X * bpCapsule.Height,
				                              bpCapsule.LocalScale.Z * bpCapsule.Radius);
				groundShape.Orientation = new Quaternion(bpCapsule.Rotation.X, bpCapsule.Rotation.Y, bpCapsule.Rotation.Z, bpCapsule.Rotation.W);
				groundShape.IsAffectedByGravity = false;
				Space.Add(groundShape);
			}

			//Sphere Colliders
			foreach (var bpSphere in colliders.Spheres)
			{
				var groundShape = new Sphere((Vector3)(Position)bpSphere.Center, bpSphere.LocalScale.X * bpSphere.Radius);
				groundShape.Orientation = new Quaternion(bpSphere.Rotation.X, bpSphere.Rotation.Y, bpSphere.Rotation.Z, bpSphere.Rotation.W);
				groundShape.IsAffectedByGravity = false;
				Space.Add(groundShape);
			}

			//Terrain Colliders
			foreach (var bpTerrain in colliders.Terrains)
			{
				var data = new float[bpTerrain.Width,bpTerrain.Height];
				for (int y = 0; y < bpTerrain.Height; y++)
				{
					for (int x = 0; x < bpTerrain.Width; x++)
					{
						data[x,y] = bpTerrain.HeightData[y * bpTerrain.Width + x];
					}
				}

				Terrain groundShape = new Terrain(data, 
				                   new AffineTransform((Vector3)(Position)bpTerrain.LocalScale, 
				                    new Quaternion(bpTerrain.Rotation.X, bpTerrain.Rotation.Y, bpTerrain.Rotation.Z, bpTerrain.Rotation.W), 
				                   (Vector3)(Position)bpTerrain.Center));

				groundShape.Shape.QuadTriangleOrganization = BEPUphysics.CollisionShapes.QuadTriangleOrganization.BottomRightUpperLeft;
				Space.Add(groundShape);
			}
			//Mesh colliders
			foreach (var bpMesh in colliders.Meshes)
			{
				List<Vector3> vList = new List<Vector3>();
				foreach (var data in bpMesh.Vertexes)
				{
					vList.Add(new Vector3(data.X, data.Y, data.Z));
				}
				StaticMesh groundShape = new StaticMesh
					(
						vList.ToArray(), 
						bpMesh.Triangles.ToArray(),
					    new AffineTransform((Vector3)(Position)bpMesh.LocalScale,
					    new Quaternion(bpMesh.Rotation.X, bpMesh.Rotation.Y, bpMesh.Rotation.Z, bpMesh.Rotation.W), 
					    (Vector3)(Position)bpMesh.Center)
					);
				Space.Add(groundShape);
			}

			f.Close();
		}

		public void Run(object threadContext)
		{
			Stopwatch timer = new Stopwatch();
			timer.Start();
			isRunning = true;

			while(isRunning)
			{
				if(timer.Elapsed < TimeSpan.FromSeconds(1/FPS))
				{
					if(Region.NumPlayers <= 0)
					{
						Thread.Sleep(1000);
						timer.Restart();
					}
					if((int)(1000f/FPS - timer.Elapsed.Milliseconds) > 0)
					{
						Thread.Sleep((int)(1000f/FPS - timer.Elapsed.Milliseconds));
					}
					continue;
				}
				var updateTime = timer.Elapsed;
				timer.Restart();
				Update(updateTime);
			}
		}

		private void Update(TimeSpan elapsed)
		{
			lock(this)
			{
				Space.Update();
			}
		}

		public void Stop()
		{
			isRunning = false;
		}
	}
}

