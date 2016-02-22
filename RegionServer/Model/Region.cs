using System.Collections.Generic;
using RegionServer.Model.Interfaces;
using System.Linq;
using System;
using ExitGames.Logging;

namespace RegionServer.Model
{
	public class Region : IPlayerListener
	{
		private Dictionary<int, IObject> _allObjects;
		private Dictionary<int, IPlayer> _allPlayers;
		public readonly ILogger Log = LogManager.GetCurrentClassLogger();


		public Region()
		{
			_allObjects = new Dictionary<int, IObject>();
			_allPlayers = new Dictionary<int, IPlayer>();
		}

		#region All Objects Functions

		public Dictionary<int, IObject>.ValueCollection VisibleObjects
		{
			get { return _allObjects.Values; }
		}

		public void AddObject(IObject obj)
		{
			if(_allObjects.ContainsKey(obj.ObjectId))
			{
				//Log that we tried to add the same object twice
				return;
			}

			_allObjects.Add(obj.ObjectId, obj);
		}

		public void RemoveObject(IObject obj)
		{
			_allObjects.Remove(obj.ObjectId);
		}

		public void RemoveObject(IEnumerable<IObject> list)
		{
			foreach (var obj in list)
			{
				if (obj != null)
				{
					_allObjects.Remove(obj.ObjectId);
				}
			}
		}

		public IObject FindObject(int objId)
		{
			if(_allObjects.ContainsKey(objId))
			{
				return _allObjects[objId];
			}
			return null;
		}

		public T FindObject<T> (T objType, int objId) where T : class, IObject
		{
			return FindObject(objId) as T;
		}

		public IObject[] AllObjectsArray()
		{
			return _allObjects.Values.ToArray();
		}

		public void ForEachObject(Action<IObject> action)
		{
			foreach (var obj in _allObjects)
			{
				action(obj.Value);
			}
		}

		#endregion

		#region All Players Functions

		public void AddPlayer(IPlayer player)
		{
			var obj = player as IObject;
			if (obj != null)
			{
				_allPlayers.Add(obj.ObjectId, player);
			}

			if(OnAddPlayer != null) 
			{
				OnAddPlayer(player);
			}
		}

		public event Action<IPlayer> OnAddPlayer;
		public event Action<IPlayer> OnRemovePlayer;

		public void RemovePlayer(IPlayer player)
		{
			var obj = player as IObject;
			if (obj != null)
			{
				_allPlayers.Remove(obj.ObjectId);
			}

			if (OnRemovePlayer != null)
			{
				OnRemovePlayer(player);
			}
		}

		public Dictionary<int, IPlayer> AllPlayers
		{
			get { return _allPlayers; }
		}

		public IPlayer[] AllPlayersArray()
		{
			return _allPlayers.Values.ToArray();
		}

		public void ForEachPlayer(Action<IPlayer> action)
		{
			foreach (var player in _allPlayers)
			{
				action(player.Value);
			}
		}

		public int NumPlayers
		{
			get { return _allPlayers.Count; }
		}

		public IPlayer GetPlayer(string name)
		{
			throw new NotImplementedException();
		}

		public IPlayer GetPlayer(int id)
		{
			if ( _allPlayers.ContainsKey(id))
			{
				return _allPlayers[id];
			}
			return null;
		}

		#endregion

		#region Visible Object Manipulation

		public void AddVisibleObject(IObject obj)
		{
			var player = obj as IPlayer;
			if (player != null)
			{
				if(GetPlayer(obj.ObjectId) == null)
				{
					AddPlayer(player);
				}
			}

			var visibles = GetVisibleObjects(obj, 2000);
			foreach (var visible in visibles)
			{
				if (visible == null)
				{
					continue;
				}

				visible.KnownList.AddKnownObject(obj);
				obj.KnownList.AddKnownObject(visible);
				
			}
		}

		public void RemoveVisibleObject(IObject obj)
		{
			if (obj == null)
			{
				return;
			}

			_allObjects.Remove(obj.ObjectId);

			ForEachObject(a => a.KnownList.RemoveKnownObject(obj));

			obj.KnownList.RemoveAllKnownObjects();

			var player = obj as IPlayer;
			if (player != null)
			{
				RemovePlayer(player);
			}
		}

		public List<IPlayable> GetVisiblePlayable(IObject obj)
		{
			return _allPlayers.Values
				.Where(a => a != null && a != obj && a is IPlayable && a is IObject && ((IObject)a).IsVisible)
				.Cast<IPlayable>()
				.ToList();
		}

		public List<IObject> GetVisibleObjects(IObject obj, float radius = 0, bool use3D = false)
		{
			if(radius == 0)
			{
				return _allObjects.Values.Where(a => a != null && a != obj && a.IsVisible).ToList();
			}

			float sqRadius = radius * radius;
			float x = obj.Position.X;
			float y = obj.Position.Y;
			float z = obj.Position.Z;

			return _allObjects.Values.Where(a => 
			                                {
												if (a == null || a == obj)
												{
													return false;
												}
												float dx = a.Position.X - x;
												float dy = use3D ? a.Position.Y - y : 0;
												float dz = a.Position.Z - z;

												if(((dx*dx) + (dy*dy) + (dz*dz)) < sqRadius)
												{
													return true;
												}
												return false;
											}).ToList();
		}

		#endregion
	}
}

