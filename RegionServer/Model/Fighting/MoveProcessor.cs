using System;
using System.Collections.Generic;
using ComplexServerCommon.MessageObjects;
using ComplexServerCommon.MessageObjects.Enums;
using ExitGames.Logging;
using RegionServer.Model.Stats;

namespace RegionServer.Model.Fighting
{
    public class MoveProcessor
    {
        private const bool DEBUG = true;
        private static readonly string CLASSNAME = "MoveProcessor";
        protected static ILogger Log = LogManager.GetCurrentClassLogger();

        private readonly Fight fight;

        private Dictionary<int, ExchangeProfile> resultForClient;

        public MoveProcessor(Fight fight)
        {
            this.fight = fight;
            resultForClient = new Dictionary<int, ExchangeProfile>();
        }

        public Dictionary<int, ExchangeProfile> ProcessMoves(FightMove playerMove, FightMove opponentMove)
        {
            var instance = fight.getParticipant(playerMove.PeerObjectId);
            var opponent = fight.getParticipant(opponentMove.PeerObjectId);

            if (instance == null || opponent == null) DebugUtils.Logp(DebugUtils.Level.FATAL, CLASSNAME, "ProcessMoves", "instance or opponent in fight.getParticipant is null");

            var playerOutcome = FightUtils.CheckAhitB(playerMove, opponentMove); //check if player A's attack has succeeded(Hit) or was blocked(Block)
            var opponentOutcome = FightUtils.CheckAhitB(opponentMove, playerMove); // -- || --

            var opponentTeamDamage = ProcessOutcome(instance, opponent, playerOutcome); //do the actual hit/block, with all the stats
            var playerTeamDamage = ProcessOutcome(opponent, instance, opponentOutcome);//do the actual hit/block, with all the stats

            fight.updateTotalTeamHealth(fight.getPlayerTeam(opponent), -opponentTeamDamage);
            fight.updateTotalTeamHealth(fight.getPlayerTeam(instance), -playerTeamDamage);

            if (DEBUG) Log.DebugFormat("{0}({1})<-dmg: {2}(team total left {3}/ {4}({5})<-dmg: {6} (team total left {7}", 
                fight.getPlayerTeam(instance), instance.ToString(), playerTeamDamage, fight.getTotalTeamHealth(fight.getPlayerTeam(instance)),
                fight.getPlayerTeam(opponent), opponent.ToString(), opponentTeamDamage, fight.getTotalTeamHealth(fight.getPlayerTeam(opponent)));

            return resultForClient;
        }

        public int ProcessOutcome(CCharacter attacker, CCharacter target, MoveOutcome outcome)
        {
            const string METHODNAME = "ProcessOutcome";

            var attackerStats = attacker.Stats;
            var minDamage = (int)attackerStats.GetStat(new MinDamage());
            var maxDamage = (int)attackerStats.GetStat(new MaxDamage());
            var damage = RngUtil.intRange(minDamage, maxDamage);

            switch (outcome)
            {
                case (MoveOutcome.Hit):
                    //HIT CHANCE VS TARGET'S DODGE CHANCE
                    if (attackerStats.GetStat(new HitChance(), target) > RngUtil.hundredRoll()) 
                    {
                        //CRIT CHANCE VS TARGET'S ANTI-CRIT CHANCE, if fails then HIT
                        damage = attackerStats.GetStat(
                            new CriticalHitChance(), target) > RngUtil.hundredRoll()    ? applyOutcome(attacker, target, MoveOutcome.Crit, damage)
                                                                                        : applyOutcome(attacker, target, MoveOutcome.Hit, damage);
                    }
                    else
                    {
                        //IF HIT 'MISSES' ITS A DODGE
                        damage = applyOutcome(attacker, target, MoveOutcome.Dodge, damage);
                    }
                    break;
                case (MoveOutcome.Block):
                    damage = applyOutcome(attacker, target,
                        attacker.Stats.GetStat(new CriticalHitChance(), target) * 0.2f > RngUtil.hundredRoll() ? MoveOutcome.BlockCrit : MoveOutcome.Block,
                        damage);
                    break;
            }

            return damage;
        }

        private int applyOutcome(CCharacter attacker, CCharacter target, MoveOutcome outcome, int damage)
        {
            switch (outcome)
            {
                case (MoveOutcome.Crit):
                    damage += (int)attacker.Stats.GetStat(new CriticalDamage());
                    break;
                case (MoveOutcome.Dodge):
                    damage = 0;
                    break;
                case (MoveOutcome.BlockCrit):
                    damage += (int)(attacker.Stats.GetStat(new CriticalDamage()) * 1.15f); //extra damage for critting through block
                    break;
                case (MoveOutcome.Block):
                    damage = 0;
                    break;
            }

            var moveLog = new ExchangeProfile()
                                {
                                    objectId = attacker.ObjectId,
                                    outcome = outcome,
                                    damage = damage,
                                    totalDamage = fight.updateTotalDamage(attacker, damage) //bonus damage is accrued before potential overkill clamp
                               };

            resultForClient.Add(moveLog.objectId, moveLog);

            //All time stats: damage++, crit++, critdamage++, hit++, dodge++, blockcrit++, block++

            return target.Stats.ApplyDamage(damage); //clamps to actual damage dealt on overkill then inflicts and returns its value
        }
    }
}
