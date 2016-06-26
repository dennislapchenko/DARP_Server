using RegionServer.Model.Interfaces;
using RegionServer.Model.KnownList;
using RegionServer.Model.Stats;


namespace RegionServer.Model
{
	public abstract class CPlayable : CCharacter, IPlayable
	{
		public CPlayable(Region region, PlayableKnownList objectKnownList, IStatHolder stats, IItemHolder items, GeneralStats genStats) 
			: base(region, objectKnownList, stats, items, genStats)
		{
		}

		public new PlayableKnownList KnownList
		{
			get	{ return ObjectKnownList as PlayableKnownList; }
			set	{ ObjectKnownList = value; }
		}
	}
}

