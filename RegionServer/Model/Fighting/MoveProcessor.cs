using System.Collections.Generic;
using ComplexServerCommon.MessageObjects;
using ComplexServerCommon.MessageObjects.Enums;
using ExitGames.Logging;
using RegionServer.Calculators;
using RegionServer.Model.Effects;
using RegionServer.Model.Stats;
using RegionServer.Model.Stats.SecondaryStats;

namespace RegionServer.Model.Fighting
{
    public class MoveProcessor
    {
        private const bool DEBUG = false;
        private static readonly string CLASSNAME = "MoveProcessor";
        protected static ILogger Log = LogManager.GetCurrentClassLogger();

        private readonly Fight fight;

        private Dictionary<int, ExchangeProfile> resultForClient;

        public MoveProcessor(Fight fight)
        {
            this.fight      = fight;
            resultForClient = new Dictionary<int, ExchangeProfile>();
        }

        public Dictionary<int, ExchangeProfile> ProcessMoves(FightMove playerMove, FightMove opponentMove)
        {
//            playerMove.SkillId   = (byte) EffectEnum.DAMAGE;
//            opponentMove.SkillId = (byte) EffectEnum.DAMAGE;

            var instance = fight.getParticipant(playerMove.PeerObjectId);
            var opponent = fight.getParticipant(opponentMove.PeerObjectId);

            if (instance == null || opponent == null) DebugUtils.Logp(DebugUtils.Level.FATAL, CLASSNAME, "ProcessMoves", "instance or opponent in fight.getParticipant is null");
        
            var playerOutcome   = CheckAhitB(playerMove, opponentMove); //check if player A's attack has succeeded(Hit) or was blocked(Block)
            var opponentOutcome = CheckAhitB(opponentMove, playerMove); // -- || --

            var playerEnv   = new SpellEnvironment(instance, playerMove.SkillId, opponent, opponentMove.SkillId);
            var opponentEnv = new SpellEnvironment(opponent, opponentMove.SkillId, instance,playerMove.SkillId);

            sendOnApplyToSpells(playerEnv);

            var opponentTeamDamage = ProcessOutcome(playerEnv, playerOutcome); //do the actual hit/block, with all the stats
            var playerTeamDamage   = ProcessOutcome(opponentEnv, opponentOutcome);//do the actual hit/block, with all the stats

            sendOnUpdateToSpells(playerEnv);

            fight.updateTotalTeamHealth(fight.getPlayerTeam(opponent), -opponentTeamDamage);
            fight.updateTotalTeamHealth(fight.getPlayerTeam(instance), -playerTeamDamage);

            if (DEBUG) Log.DebugFormat("{0}({1})<-dmg: {2}(team total left {3}/ {4}({5})<-dmg: {6} (team total left {7}", 
                fight.getPlayerTeam(instance), instance.ToString(), playerTeamDamage, fight.getTotalTeamHealth(fight.getPlayerTeam(instance)),
                fight.getPlayerTeam(opponent), opponent.ToString(), opponentTeamDamage, fight.getTotalTeamHealth(fight.getPlayerTeam(opponent)));

            return resultForClient;
        }

        private void sendOnApplyToSpells(SpellEnvironment env)
        {
            env.Character.Effects.UseSpell(env.CharacterSpell);
            env.Target.Effects.UseSpell(env.TargetSpell);
        }

        private void sendOnUpdateToSpells(SpellEnvironment env)
        {
            env.Character.Effects.OnUpdate();
            env.Target.Effects.OnUpdate();
        }

        public int ProcessOutcome(SpellEnvironment env, MoveOutcome outcome)
        {
            const string METHODNAME = "ProcessOutcome";
            var attacker = env.Character;
            var target   = env.Target;

            var attackerStats = attacker.Stats;
            var damage = (int)attackerStats.GetStat(new Damage(), target); //calculates rng damage (minDmg <= dmg <= maxDmg) and applies target negation

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
                        attacker.Stats.GetStat(new CriticalHitChance(), target) * 0.2f > RngUtil.hundredRoll()  ? MoveOutcome.BlockCrit 
                                                                                                                : MoveOutcome.Block,
                        damage);
                    break;
            }

            return damage;
        }

        private int applyOutcome(CCharacter attacker, CCharacter target, MoveOutcome outcome, int damage)
        {
            switch (outcome)
            {
                case (MoveOutcome.Hit):
                    break;
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
            attacker.Effects.OnDamageGiven(damage, attacker);

            //target.Effects.OnDamageReceived is called inside Stats.ApplyDamage
            return target.Stats.ApplyDamage(damage, attacker); //clamps to actual damage dealt on overkill then inflicts and returns its value
        }

        public static MoveOutcome CheckAhitB(FightMove A, FightMove B)
        {
            return (A.AttackSpot != B.BlockSpots[0] && A.AttackSpot != B.BlockSpots[1]) 
                ? MoveOutcome.Hit 
                : MoveOutcome.Block;
        }
    }
}
