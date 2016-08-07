using System.Collections.Generic;
using RegionServer.Model.CharacterDatas;
using RegionServer.Model.Effects;
using RegionServer.Model.Interfaces;
using RegionServer.Model.KnownList;


namespace RegionServer.Model
{
	public abstract class CPlayable : CCharacter, IPlayable
	{
		public CPlayable(Region region, PlayableKnownList objectKnownList, IStatHolder stats, IItemHolder items, EffectHolder effects, IEnumerable<ICharacterData> charData) 
			: base(region, objectKnownList, stats, items, effects, charData)
		{
		}

		public new PlayableKnownList KnownList
		{
			get	{ return ObjectKnownList as PlayableKnownList; }
			set	{ ObjectKnownList = value; }
		}
	}
}

