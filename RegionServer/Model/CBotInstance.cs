using System;
using System.Collections.Generic;
using ComplexServerCommon.MessageObjects;
using RegionServer.Model.CharacterDatas;
using RegionServer.Model.Effects;
using RegionServer.Model.Fighting;
using RegionServer.Model.Interfaces;
using RegionServer.Model.KnownList;
using RegionServer.Model.Stats;
using RegionServer.Model.Stats.BaseStats;

namespace RegionServer.Model
{
	public class CBotInstance : CCharacter, IBot
	{
		private static readonly string CLASSNAME = "CBotInstance";

	    private static readonly List<string> botNames = new List<string>()
	    {
	        "Alberto", "Gandolfini", "ISIS BOI", "CHEGRILLA", "1337 BOY", "EBLAN",
            "DANILO", "DANIELA", "ARTEMON", "PETROSYAN", "KOTUK", "LANDYSH",
            "GERYCH", "KOKYCH", "MRAZJ", "GUCCI", "ARMANI", "LEONTYEV",
        };

	    public delegate CBotInstance Factory(byte level);

		public CBotInstance(byte level, Region region, CharacterKnownList objectKnownList, StatHolder stats, IItemHolder items, EffectHolder effects,
            IEnumerable<ICharacterData> charData) 
			: base(region, objectKnownList, stats, items,effects, charData)
        {
            Stats.SetStat<Level>(level);
            ObjectId = Math.Abs(Guid.NewGuid().GetHashCode());
            Name = botNames[new Random().Next(0, botNames.Count)];
        }

		public void configureBot()
		{
		    var level = (byte)Stats.GetStat<Level>();
			CharacterData[typeof(GeneralStats)] = FightUtils.getRandomGenStats(GetCharData<GeneralStats>(), level);
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
//            var timer = new Stopwatch();
//            timer.Start();
//            var delayInMs = RngUtil.intRange(0, (int)moveReplyMaxDelayInMs);
//		    while (timer.ElapsedMilliseconds < delayInMs) continue;

            var newMove = new FightMove
									{
										PeerObjectId = this.ObjectId,
										AttackSpot = FightUtils.getRandomHit(),
										BlockSpots = FightUtils.getRandomBlock(),
										TargetObjectId = targetId
									};

			CurrentFight.AddMoveSendPkg(this, newMove);
		}
        public void makeAMove()
        {
            if (CurrentFight.hasMovesAgainstAll(this)) return;
            var newMove = new FightMove
                        {
                            PeerObjectId = this.ObjectId,
                            AttackSpot = FightUtils.getRandomHit(),
                            BlockSpots = FightUtils.getRandomBlock(),
                            SkillId = 0,
                            TargetObjectId = Target.ObjectId
                        };
            DebugUtils.Logp(DebugUtils.Level.INFO, CLASSNAME, "makeAMove", "bot submitting a move");
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
