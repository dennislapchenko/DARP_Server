using System;
using RegionServer.Model.Interfaces;
using System.Collections.Generic;
using System.Linq;


namespace RegionServer.Model.KnownList
{
	public class ObjectKnownList : IKnownList
	{
		protected Dictionary<int, IObject> KnownObjects;

		public IObject Owner {get; set;}

		public ObjectKnownList()
		{
			KnownObjects = new Dictionary<int, IObject>();
		}

		public virtual bool AddKnownObject(IObject obj)
		{
			if(obj == null)
			{
				return false;
			}
			//if Owner.InstanceId == -1 - Assume it is a GM
			if(Owner.InstanceId != -1 && obj.InstanceId != Owner.InstanceId)
			{
				return false;
			}
			if(KnowsObject(obj))
			{
				return false;
			}

			KnownObjects.Add(obj.ObjectId, obj);
			return true;

		}
		public bool KnowsObject(IObject obj)
		{
			return Owner == obj || KnownObjects.ContainsKey(obj.ObjectId);
		}

		public virtual void RemoveAllKnownObjects()
		{
			KnownObjects.Clear(); //upon teleport 
		}

		public virtual bool RemoveKnownObject(IObject obj)
		{
			if (obj == null)
			{
				return false;
			}

			return KnownObjects.Remove(obj.ObjectId);
		}

		public virtual void FindObjects() //to be used in other lists
		{
		}

		public virtual void ForgetObjects(bool fullCheck)
		{
			var values = KnownObjects.Values.ToList();
			var iter = values.GetEnumerator();
			while(iter.MoveNext())
			{
				if(iter.Current == null)
				{
					values.Remove(iter.Current);
					continue;
				}

				if(!fullCheck && !(iter.Current is IPlayable)) //
				{
					continue;
				}

				if(!iter.Current.IsVisible || !Util.IsInShortRange(DistanceToForgetObject(iter.Current), Owner, iter.Current, true))
				{
					values.Remove(iter.Current);
				}
			}
			Dictionary<int, IObject> newKnownObjects = new Dictionary<int, IObject>();
			foreach (var value in values)
			{
				newKnownObjects.Add(value.ObjectId, value);
			}
			KnownObjects = newKnownObjects;
		}

		public virtual int DistanceToForgetObject(IObject obj) { return 0; }

		public virtual int DistanceToWatchObject(IObject obj) { return 0; }

	
	}
}

