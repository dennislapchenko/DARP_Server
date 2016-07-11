using System;
using System.Collections.Generic;
using System.Diagnostics;
using ComplexServerCommon.MessageObjects;
using RegionServer.Model.Fighting;
using RegionServer.Model.Interfaces;
using RegionServer.Model.Items;
using RegionServer.Model.KnownList;
using RegionServer.Model.Stats;

namespace RegionServer.Model
{
	public class CBotInstance : CCharacter, IBot
	{
		private static readonly string CLASSNAME = "CBotInstance";

	    private static readonly List<string> botNames = new List<string>() {"Alberto", "Gandolfini", "ISIS BOI", "CHEGRILLA", "1337 BOY", "EBLAN"};

	    private static readonly long moveReplyMaxDelayInMs = 7000;

	    public delegate CBotInstance Factory(byte level);

		public CBotInstance(byte level, Region region, CharacterKnownList objectKnownList, IStatHolder stats, IItemHolder items, GeneralStats genStats) 
			: base(region, objectKnownList, stats, items, genStats)
        {
            Stats.SetStat<Level>(level);
            ObjectId = Math.Abs(Guid.NewGuid().GetHashCode());
            Name = botNames[new Random().Next(0, botNames.Count)];
            GenStats.Name = Name;
        }

		public void configureBot()
		{
		    var level = (byte)Stats.GetStat<Level>();
			GenStats = FightUtils.getRandomGenStats(GenStats, level);
            Items = FightUtils.getRandomItemsByLevel(Items, level);
            Stats = FightUtils.getRandomStatsByLevel(Stats, level);
		}

	    public bool isNPC { get { return true;} }

	    public void joinQueue(IFight fight)
		{
			var METHODNAME = "joinQueue";

		    if (fight.addPlayer(this))
		    {
		        Position = fight.FightLocation;
		    }
            else
            { 
				Log.DebugFormat("{0} - {1} Failed to add bot to fight", CLASSNAME, METHODNAME);
			}
		}

		public void makeAMove(int targetId)
		{
            var timer = new Stopwatch();
            timer.Start();
            var delayInMs = RngUtil.intRange(0, (int)moveReplyMaxDelayInMs);
		    while (timer.ElapsedMilliseconds < delayInMs) continue;

            var newMove = new FightMove
									{
										PeerObjectId = this.ObjectId,
										AttackSpot = FightUtils.getRandomHit(),
										BlockSpots = FightUtils.getRandomBlock(),
										TargetObjectId = targetId
									};

			CurrentFight.AddMoveSendPkg(this, newMove);
		}

	    public void configureBot(byte level)
	    {
	        throw new NotImplementedException();
	    }

//	    public CBotInstance copy()
//		{
//			return new CBotInstance(Region, new CharacterKnownList(), Stats, new ItemHolder(ItemDBCache.instance), new GeneralStats());
//		}

	}
}
