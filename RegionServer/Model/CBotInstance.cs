using System;
using System.Runtime.CompilerServices;
using ComplexServerCommon.Enums;
using ComplexServerCommon.MessageObjects;
using FluentNHibernate.Utils;
using RegionServer.Model.Interfaces;
using RegionServer.Model.KnownList;
using RegionServer.Model.Stats;

namespace RegionServer.Model
{
	public class CBotInstance : CCharacter, IBot
	{
		public static readonly string CLASSNAME = "CBotInstance";

		public CBotInstance(Region region, CharacterKnownList objectKnownList, IStatHolder stats, IItemHolder items, GeneralStats genStats) 
			: base(region, objectKnownList, stats, items, genStats)
		{
		}

		public CBotInstance()
		{
		}

		public void configureBot(byte level)
		{
			ObjectId = new Guid().GetHashCode();
			Name = "Bot " + ObjectId;
			GenStats.Name = Name;

			GenStats = FightUtils.getRandomGenStats(GenStats, level);
			Stats = FightUtils.getRandomStatsByLevel(Stats, level);
			Items = FightUtils.getRandomItemsByLevel(Items, level);
		}

		public void joinQueue(IFight fight)
		{
			string METHODNAME = "joinQueue";

			if (!fight.addPlayer(this))
			{ 
				Log.DebugFormat("{0} - {1} Failed to add bot to fight", CLASSNAME, METHODNAME);
			}
		}

		public void makeAMove()
		{
			var newMove = new FightMove
									{
										PeerObjectId = this.ObjectId,
										AttackSpot = FightUtils.getRandomHit(),
										BlockSpots = FightUtils.getRandomBlock(),
										TargetObjectId = this.Target.ObjectId
									};

			CurrentFight.AddMoveSendPkg(this, newMove);
		}

		public CBotInstance copy()
		{
			return new CBotInstance(Region, (CharacterKnownList)ObjectKnownList, Stats, Items, GenStats);
		}

	}
}
