using System;
using RegionServer.Model.Interfaces;
using RegionServer.Model.ServerEvents;


namespace RegionServer.Model.KnownList
{
	public class PlayerKnownList : PlayableKnownList
	{

		public override bool AddKnownObject(IObject obj)
		{
			if (!base.AddKnownObject(obj))
			{
				return false;
			}

			obj.SendInfo(Owner);

			ICharacter character = obj as ICharacter;
			if(character != null)
			{
				character.SendStateToPlayer(Owner);
			}
			return true;
		}

		public override bool RemoveKnownObject(IObject obj)
		{
			if (!base.RemoveKnownObject(obj))
			{
				return false;
			}

			Owner.SendPacket(new DeleteObject(obj));
			return true;
		}

		public override int DistanceToForgetObject(IObject obj)
		{
			if(KnownObjects.Count <= 25)
			{
				return 400;
			}

			if(KnownObjects.Count <= 35)
			{
				return 350;
			}
			if(KnownObjects.Count <= 70)
			{
				return 295;
			}

			return 235;
		}

		public override int DistanceToWatchObject(IObject obj)
		{
			if(KnownObjects.Count <= 25)
			{
				return 340;
			}

			if(KnownObjects.Count <= 35)
			{
				return 290;
			}

			if(KnownObjects.Count <= 70)
			{
				return 230;
			}
			return 170;
		}

	}
}

