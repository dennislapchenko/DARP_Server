using RegionServer.Model.Interfaces;
using RegionServer.Model.KnownList;


namespace RegionServer.Model
{
	public abstract class CPlayable : CCharacter, IPlayable
	{
		public CPlayable(Region region, PlayableKnownList objectKnownList) : base(region, objectKnownList)
		{
		}

		public new PlayableKnownList KnownList
		{
			get	{ return ObjectKnownList as PlayableKnownList; }
			set	{ ObjectKnownList = value; }
		}
	}
}

