using RegionServer.Model.Interfaces;
using RegionServer.Model.KnownList;
using ComplexServerCommon;
using RegionServer.Model.ServerEvents;


namespace RegionServer.Model
{
	public class CObject : IObject
	{
		public int InstanceId { get; set; }
		public int ObjectId { get; set; }
		public bool IsVisible { get; set; }
		public string Name { get; set; }
		public virtual Position Position { get; set; }
		protected ObjectKnownList ObjectKnownList;

		private readonly Region _region;
		public Region Region { get { return _region; } }

		public virtual ObjectKnownList KnownList
		{
			get {return ObjectKnownList as ObjectKnownList;}
			set { ObjectKnownList = value;}
		}

		public CObject(Region region, ObjectKnownList objectKnownList)
		{
			_region = region;
			ObjectKnownList = objectKnownList;
			ObjectKnownList.Owner = this;
			Position = new Position();
		}

		public CObject()
		{ }



		public void ToggleVisible()
		{
			if (IsVisible)
				Decay();
			else
				Spawn();
		}
		
		public void Spawn()
		{
			IsVisible = true;

			//Region Code
			Region.AddObject(this);
			Region.AddVisibleObject(this);

			OnSpawn();
		}

		public virtual void OnSpawn()
		{
		}

		public void Decay()
		{
			IsVisible = false;

			//Region Code
			Region.RemoveObject(this);
			Region.RemoveVisibleObject(this);
		}

		public virtual void SendPacket(ServerPacket packet)
		{
		}

		public virtual void SendPacket(SystemMessageId id)
		{
		}

		public virtual void SendInfo(IObject obj)
		{
		}

	}
}

