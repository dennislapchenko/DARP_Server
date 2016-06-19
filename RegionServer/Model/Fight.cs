using RegionServer.Model.Interfaces;
using ComplexServerCommon.Enums;
using System.Collections.Generic;
using System;
using ComplexServerCommon.MessageObjects;
using System.Linq;
using RegionServer.Model.ServerEvents;
using ExitGames.Logging;
using ComplexServerCommon.MessageObjects.Enums;
using RegionServer.Model.Stats;

namespace RegionServer.Model
{
	public class Fight : IFight
	{
		private readonly bool DEBUG = true;

		private readonly Guid _fightId;

		public Guid FightId {get { return _fightId;} set{} }
		public string Creator {get;set;}
		public FightState State {get; set;}
		public FightType Type {get; set;}
		public int TeamSize {get; set;}
		public float Timeout {get; set;}
		public bool Sanguinary {get; set;}
		public Dictionary<CPlayerInstance, CurrentFightCharData> CharFightData {get;set;}
		public Dictionary<int, CCharacter> TeamBlue {get; set;} //int as objId
		public Dictionary<int, CCharacter> TeamRed {get; set;} 
		public Dictionary<int, bool> ReadyPlayers = new Dictionary<int, bool>();

		public Dictionary<FightTeam, int> TotalTeamHealth {get;set;}

		public List<FightMove> Moves {get;set;}
		public Dictionary<int, Dictionary<int, ExchangeProfile>> MoveExchanges {get;set;}
		public int ExchangeCount {get;set;}

		public delegate Fight Factory(Guid fightId, string creator, FightType type, int teamSize, float timeout);
		protected ILogger Log = LogManager.GetCurrentClassLogger();

		public Dictionary<int, CPlayerInstance> Players
		{
			get 
			{	
				var result = new Dictionary<int, CPlayerInstance>();
				foreach(var player in TeamBlue.Values)
				{
					var cplayer = player as CPlayerInstance;
					if(cplayer != null)
					{
						result.Add(cplayer.ObjectId, cplayer);
					}
				}
				foreach(var player in TeamRed.Values)
				{
					var cplayer = player as CPlayerInstance;
					if(cplayer != null)
					{
						result.Add(cplayer.ObjectId, cplayer);
					}				
				}
				return result;
			}
			set{}
		}

		public Fight(Guid fightId, string creator, FightType type, int teamSize, float timeout, bool sanguinary)
		{
			CharFightData = new Dictionary<CPlayerInstance, CurrentFightCharData>();
			TeamBlue = new Dictionary<int, CCharacter>();
			TeamRed = new Dictionary<int, CCharacter>();
			TotalTeamHealth = new Dictionary<FightTeam, int>() { {FightTeam.Red, 0}, {FightTeam.Blue, 0},};
			Moves = new List<FightMove>();
			MoveExchanges = new Dictionary<int, Dictionary<int, ExchangeProfile>>();
			ExchangeCount = 0;
			_fightId = fightId;
			Creator = creator;
			State = FightState.QUEUE;
			Type = type;
			TeamSize = teamSize;
			Timeout = timeout;
			Sanguinary = sanguinary;
		}

		#region QUEUE OPERATIONS
		public bool AddPlayer(int team, CCharacter player) //TODO: player has to choose team, when GROUP fight
		{
			var targetTeam = (team != 1) ? TeamBlue : TeamRed;
			if(player.CurrentFight == null)
			{
				targetTeam.Add(player.ObjectId, player);
				player.CurrentFight = this;
				CharFightData.Add((CPlayerInstance)player, new CurrentFightCharData(){Team = (team != 1) ? FightTeam.Blue : FightTeam.Red, TotalDamage = 0, WLT = null});
				TotalTeamHealth[(team != 1) ? FightTeam.Blue : FightTeam.Red] += (int)player.Stats.GetStat(new MaxHealth());
				Log.DebugFormat("({0} added to {1}) blue team HP: {2} / red team HP: {3}", player.Name,(team != 1) ? FightTeam.Blue : FightTeam.Red, TotalTeamHealth[FightTeam.Blue], TotalTeamHealth[FightTeam.Red]);
				ShouldSendReadyPrompts();
				return true;
			}
			return false;
		}
		private void ShouldSendReadyPrompts()
		{
			if(Players.Count == TeamSize*2)
			{
				foreach(var p in Players.Values)
				{
					p.SendPacket(new ReadyPrompt());
				}
			}
		}
		public void SetPlayerReady(int objId, bool status)
		{
			if(!ReadyPlayers.ContainsKey(objId))
			{
				ReadyPlayers.Add(objId, status);
			}
			CheckIfAllReady();
		}
		private void CheckIfAllReady()
		{
			if(ReadyPlayers.Count == TeamSize*2)
			{
				State = FightState.ENGAGED;

				foreach(var player in Players.Values)
				{
					player.SwitchCurrentFightTarget(); //set every players target (to a player they havent attacked yet
					Log.DebugFormat("{0}'s fight data: {1}", player.Name, CharFightData[player]);
					player.SendPacket(new StartFight()); //Load fight scene event for clients
				}
				ReadyPlayers = new Dictionary<int, bool>();
			}
		}
		public bool RemovePlayer(CCharacter player) //improve to a teamless argument
		{
			bool success;
			success = TeamRed.Remove(player.ObjectId);
			if(!success)
			{
				success = TeamBlue.Remove(player.ObjectId);
				player.CurrentFight = null;
				return true;
			}
			if(success)
			{
				player.CurrentFight = null;
				return true;
			}
			return false;
		}
		public CCharacter GetPlayer(int objectId)
		{
			CCharacter result;
			if(!TeamRed.TryGetValue(objectId, out result))
			{
				TeamBlue.TryGetValue(objectId, out result);
			}
			if(result != null)
			{
				return result;
			}
			return null;
		}
		public int NumPlayers()
		{
			return TeamRed.Count + TeamBlue.Count;
		}
		#endregion

		#region FIGHT OPERATIONS
		public void AddMoveSendPkg(CPlayerInstance addingInstance, FightMove newMove)
		{
			if(IsDuplicateMove(newMove)) { return; }

			Moves.Add(newMove);
			if(DEBUG) {Log.DebugFormat("Adding move by: {0}.A: {1}, B: {2}-{3}, target: {4}",newMove.PeerObjectId, newMove.AttackSpot, newMove.BlockSpots[0], newMove.BlockSpots[1], newMove.TargetObjectId);}

			var allMoves = new List<FightMove>(Moves);
			foreach(var move in allMoves)
			{
				if(DEBUG) {Log.DebugFormat("Moves.move by: {0}.A: {1}, B: {2}-{3}, target: {4}",move.PeerObjectId, move.AttackSpot, move.BlockSpots[0], move.BlockSpots[1], move.TargetObjectId);}
				if(newMove.TargetObjectId == move.PeerObjectId && newMove.PeerObjectId == move.TargetObjectId)
				{
					ProcessMoves(newMove, move);
					Moves.Remove(newMove);
					Moves.Remove(move);
					if(State == FightState.FINISHED) { return; }
					DeadPlayerGarbageCollection();
				}
			}
			addingInstance.SwitchCurrentFightTarget();
			addingInstance.SendPacket(new FightParticipants(addingInstance, true));
		}

		private void DeadPlayerGarbageCollection()
		{
			foreach(var player in Players)
			{
				//if dead delete all moves where this dead player is present either as ATTACKER or as DEFENDER
				if(player.Value.IsDead)
				{
					Log.DebugFormat("A dead player: {0}", player.Value.Name);
					lock(this)
					{
						var moves = new List<FightMove>(Moves);
						foreach(var move in moves)
						{
							if(move.PeerObjectId == player.Key || move.TargetObjectId == player.Key)
							{
								Log.DebugFormat("moves involving this dead player: {0}", move);
								Moves.Remove(move);
							}
						}
					}
				}
				//if not dead - check if target is dead and assign new if dead
				else
				{
					Log.DebugFormat("checking if player {0} has a dead target", player.Value.Name);
					var playersTarget = player.Value.Target as CCharacter;
					Log.DebugFormat("got a target as CCharacter : {0}", (playersTarget != null));
					if(playersTarget != null)
					{
						if(playersTarget.IsDead)
						{
							Log.DebugFormat("target is dead, sending player to target switcher");
							player.Value.SwitchCurrentFightTarget();
							player.Value.SendPacket(new FightParticipants(player.Value, true));
						}
					}
				} 
			}
		}

		private bool IsDuplicateMove(FightMove move)
		{
			foreach(var m in Moves)
			{
				if(m.PeerObjectId == move.PeerObjectId && m.TargetObjectId == move.TargetObjectId)
				{
					return true;
				}
			}
			return false;
		}

		public void ProcessMoves(FightMove playerMove, FightMove opponentMove)
		{
			var instance = GetPlayer(playerMove.PeerObjectId) as CPlayerInstance;
			var opponent = GetPlayer(opponentMove.PeerObjectId) as CPlayerInstance;
			var playerOutcome = Util.CheckAhitB(playerMove, opponentMove); //check if player A's attack has succeeded(Hit) or was blocked(Block)
			var opponentOutcome = Util.CheckAhitB(opponentMove, playerMove); // -- || --

			var result = new Dictionary<int, ExchangeProfile>(); //client fight log item

			var opponentAteamHealth = ProcessOutcome(instance, opponent, playerOutcome, result); //do the actual hit/block, with all the stats
			var opponentBteamHealth = ProcessOutcome(opponent, instance, opponentOutcome, result);//do the actual hit/block, with all the stats

			MoveExchanges.Add(++ExchangeCount, result); //add exchange result to the Fight log
			if(DEBUG) Log.DebugFormat("teamA({0}) hp: {1}/ teamB({2}) hp: {3}", instance.Name, opponentAteamHealth, opponent.Name, opponentBteamHealth);

			foreach(var player in Players)
			{
				player.Value.SendPacket(new FightUpdate(player.Value, result));
			}
			CheckForFinish(instance, opponentAteamHealth, opponentBteamHealth);
		}

		public int ProcessOutcome(CPlayerInstance attacker, CPlayerInstance target, MoveOutcome outcome, Dictionary<int, ExchangeProfile> result)
		{
			var rgen = new Random();

			var log = new ExchangeProfile(attacker.ObjectId, 0, 0);
			var attackerStats = attacker.Stats;
			var targetStats = target.Stats;
			var minDamage = (int)attackerStats.GetStat(new MinDamage());
			var maxDamage = (int)attackerStats.GetStat(new MaxDamage());
			var damage = rgen.Next(minDamage, maxDamage+1);

			switch(outcome)
			{
				case(MoveOutcome.Hit):
					//HIT CHANCE VS TARGET'S DODGE CHANCE
					if(attackerStats.GetStat(new HitChance(), target) > rgen.Next(0,100))
					{
						//CRIT CHANCE VS TARGET'S ANTI-CRIT CHANCE
						if(attackerStats.GetStat(new CriticalHitChance(), target) > rgen.Next(0,100))
						{
							//CRITICALLY HIT
							damage += (int)attackerStats.GetStat(new CriticalDamage());
							var totalDamage = UpdateTotalDamage(attacker, damage);

							log.outcome = MoveOutcome.Crit;
							log.damage = damage;
							log.totalDamage = totalDamage;

							damage = targetStats.ApplyDamage(damage);

							if(DEBUG) Log.DebugFormat("CLOG CRIT: ({0}){1} -> ({2}){3}\n : {4} = {5}({6})(real dmg: {7})", log.objectId, attacker.Name, target.ObjectId, target.Name, log.outcome.ToString(), log.damage.ToString(), log.totalDamage.ToString(), damage);
						}
						else //HIT
						{
							var totalDamage = UpdateTotalDamage(attacker, damage); //bonus damage is accrued before possible overkill clamp
							log.outcome = MoveOutcome.Hit;
							log.damage = damage;
							log.totalDamage = totalDamage;

							damage = targetStats.ApplyDamage(damage); //clamps to actual damage dealt on overkill

							if(DEBUG) Log.DebugFormat("CLOG HIT: ({0}){1} -> ({2}){3}\n : {4} = {5}({6})(real dmg: {7})", log.objectId, attacker.Name, target.ObjectId, target.Name, log.outcome.ToString(), log.damage.ToString(), log.totalDamage.ToString(), damage);

							//All time stats: hits++, Damage += damage;
						}
					}
					else
					{

						log.outcome = MoveOutcome.Dodge;
						log.damage = 0;
						damage = 0;
						log.totalDamage = UpdateTotalDamage(attacker, damage);


						if(DEBUG) Log.DebugFormat("CLOG DODGE: ({0}){1} -> ({2}){3}\n : {4} = {5}({6})", log.objectId, attacker.Name, target.ObjectId, target.Name, log.outcome.ToString(), log.damage.ToString(), log.totalDamage.ToString());
					}
					break;
				case(MoveOutcome.Block):
					if((attacker.Stats.GetStat(new CriticalHitChance(), target))*0.2f > rgen.Next(0,100)) //should be very small
					{
						//CRIT THROUGH BLOCK
						damage += (int)(attackerStats.GetStat(new CriticalDamage()) * 1.15f); //extra damage for critting through block
						var totalDamage = UpdateTotalDamage(attacker, damage);

						log.outcome = MoveOutcome.BlockCrit;
						log.damage = damage;
						log.totalDamage = totalDamage;

						damage = targetStats.ApplyDamage(damage);

						if(DEBUG) Log.DebugFormat("CLOG BLOCKCRIT:\n ({0}){1} -> ({2}){3}\n : {4} = {5}({6})(real dmg: {7})", log.objectId, attacker.Name, target.ObjectId, target.Name, log.outcome.ToString(), log.damage.ToString(), log.totalDamage.ToString(), damage);

						//All time stats: crit damage += damage, block crits++, Damage += damage;
					}
					else
					{
						//BLOCKED
						// All time stats: block++;
						log.outcome = MoveOutcome.Block;
						log.damage = 0;
						damage = 0;
						log.totalDamage = UpdateTotalDamage(attacker, damage);


						if(DEBUG) Log.DebugFormat("CLOG BLOCK: ({0}){1} -> ({2}){3}\n : {4} = {5}({6})", log.objectId, attacker.Name, target.ObjectId, target.Name, log.outcome.ToString(), log.damage.ToString(), log.totalDamage.ToString());
					}
					break;
				default:
					Log.DebugFormat("SendMoveHandler.ProcessOutcome - moveoutcome unknown");
					break;

			}
			result.Add(log.objectId, log);
			UpdateTotalTeamHealth(CharFightData[target].Team, damage);

			return TotalTeamHealth[CharFightData[target].Team];
		}

		private void CheckForFinish(CPlayerInstance instance, int hpA, int hpB)
		{
			if(hpA <= 0 && hpB <= 0)
			{
				foreach(var player in Players.Values)
				{
					var result = CharFightData[player];
					result.WLT = FightWinLossTie.Tie;
					CharFightData[player] = result;
				}
				FinishFight();
			}
			else if (hpA <= 0)
			{
				var tempPlayers = Players;
				foreach(var wonPlayer in Players.Where(t => CharFightData[t.Value].Team == CharFightData[instance].Team))
				{
					var result = CharFightData[wonPlayer.Value];
					result.WLT = FightWinLossTie.Win;
					CharFightData[wonPlayer.Value] = result;
					tempPlayers.Remove(wonPlayer.Key);
				}
				foreach(var lostPlayer in tempPlayers)
				{
					var result = CharFightData[lostPlayer.Value];
					result.WLT = FightWinLossTie.Loss;
					CharFightData[lostPlayer.Value] = result;
				}
				FinishFight();
			}
			else if(hpB <= 0)
			{
				var tempPlayers = Players;
				foreach(var wonPlayer in Players.Where(t => CharFightData[t.Value].Team != CharFightData[instance].Team))
				{
					var result = CharFightData[wonPlayer.Value];
					result.WLT = FightWinLossTie.Win;
					CharFightData[wonPlayer.Value] = result;
					tempPlayers.Remove(wonPlayer.Key);
				}
				foreach(var lostPlayer in tempPlayers)
				{
					var result = CharFightData[lostPlayer.Value];
					result.WLT = FightWinLossTie.Loss;
					CharFightData[lostPlayer.Value] = result;
				}
				FinishFight();
			}
		}


		public int UpdateTotalDamage(CPlayerInstance obj, int value)
		{
			var info = CharFightData[obj];
			info.TotalDamage += value;
			CharFightData[obj] = info;
			return info.TotalDamage;
		}
		public void UpdateTotalTeamHealth(FightTeam team, int damage)
		{
			var health = TotalTeamHealth[team];
			health -= damage;
			Log.DebugFormat("TeamHPupdate: current: {0}, dmg to be inflicted: {1}, new: {2}", TotalTeamHealth[team], damage, health);
			TotalTeamHealth[team] = health;
		}
		#endregion

		#region FINISHING FIGHT
		public void FinishFight()
		{
			Log.DebugFormat("finishing fight");
			State = FightState.FINISHED;
			foreach(var player in Players)
			{
				var reward = CalculateAndGrantRewards(player.Value);
				UpdateGenStats(player.Value, reward);
				player.Value.SendPacket(new FinishFight(reward));
				player.Value.CurrentFight = null;
				player.Value.Resurrect();
			}

			//store fight history in archive
		}
		private Rewards CalculateAndGrantRewards(CPlayerInstance instance)
		{
			var reward = new Rewards();
			Log.DebugFormat("{0} has ended the fight with a {1}", instance.Name, instance.CurrentFight.CharFightData[instance].WLT);
			switch(instance.CurrentFight.CharFightData[instance].WLT)
			{
				case(FightWinLossTie.Win):
					reward.Experience = (int)(CharFightData[instance].TotalDamage * (instance.Stats.GetStat<Level>()*1.15f));
					reward.Gold = (int)(new Random().Next(0,2) < 1 ? (new Random().Next(1,14) * instance.Stats.GetStat<Level>()) : 0);
					reward.WLT = FightWinLossTie.Win;
					reward.TotalDamage = instance.CurrentFight.CharFightData[instance].TotalDamage;
					if(CharFightData[instance].Injuried) 
					{ 
						/*instance.ApplyInjury(time);*/ 
						reward.Injury = true;
					}
					Log.DebugFormat("WON: Reward:{0} exp:{1}, gold:{2}, win:{3}, dmg:{4}", instance.ObjectId, reward.Experience, reward.Gold, reward.WLT, reward.TotalDamage);
					break;
				case(FightWinLossTie.Loss):
				case(FightWinLossTie.Tie):
					reward.Experience = (int)(CharFightData[instance].TotalDamage * 0.5f);

					reward.Gold = (int)(new Random().Next(0,1000) < 1 ? (3*instance.Stats.GetStat<Level>()) : 0);
					reward.WLT = (instance.CurrentFight.CharFightData[instance].WLT == FightWinLossTie.Loss) ? FightWinLossTie.Loss : FightWinLossTie.Tie;
					reward.TotalDamage = instance.CurrentFight.CharFightData[instance].TotalDamage;
					if(CharFightData[instance].Injuried) 
						{ 
						/*instance.ApplyInjury(time);*/ 
						reward.Injury = true;
					}
					Log.DebugFormat("{0}: Reward:{1} exp:{2}, gold:{3}, win:{4}, dmg:{5}",instance.CurrentFight.CharFightData[instance].WLT, instance.ObjectId, reward.Experience, reward.Gold, reward.WLT, reward.TotalDamage);
					break;
			}
			return reward;
		}
		private void UpdateGenStats(CPlayerInstance player, Rewards reward)
		{
			var genStats = player.GenStats;
			genStats.AddExperience((int)reward.Experience);
			genStats.Gold += (int)reward.Gold;
			genStats.Battles++;
			switch(CharFightData[player].WLT)
			{
				case(FightWinLossTie.Win): genStats.Win++;
					break;
				case(FightWinLossTie.Loss): genStats.Loss++;
					break;
				case(FightWinLossTie.Tie): genStats.Tie++;
					break;
			}
		}
		#endregion

		#region CLIENT PACKAGE INFO
		public FightQueueListItem GetQueueInfo()
		{
			var result = new FightQueueListItem()
												{
													FightId = this._fightId.ToString(),
													Type = this.Type,
													Creator = this.Creator,
													Blue = TeamBlue.Select(p => p.Value.Name).ToList(),
													Red = TeamRed.Select(p => p.Value.Name).ToList(),
													Timeout = this.Timeout,
													TeamSize = this.TeamSize,
													Sanguinary = this.Sanguinary,
												};
			return result;
		}
		#endregion
	}
}

