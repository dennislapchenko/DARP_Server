﻿using RegionServer.Model.Interfaces;using ComplexServerCommon.Enums;using System.Collections.Generic;using System;using ComplexServerCommon.MessageObjects;using System.Linq;using System.Security.Cryptography;using RegionServer.Model.ServerEvents;using ExitGames.Logging;using ComplexServerCommon.MessageObjects.Enums;using RegionServer.Model.Stats;namespace RegionServer.Model{	public class Fight : IFight	{		private readonly bool DEBUG = true;		private readonly Guid _fightId;		public Guid FightId {get { return _fightId;} set{} }		public string Creator {get;set;}		public FightState State {get; set;}		public FightType Type {get; set;}		public int TeamSize {get; set;}		public float Timeout {get; set;}		public bool Sanguinary {get; set;}		public bool BotsWelcome { get; set; }		public Dictionary<CCharacter, CurrentFightCharData> CharFightData {get;set;}		public Dictionary<int, CCharacter> TeamBlue {get; set;} //int as objId		public Dictionary<int, CCharacter> TeamRed {get; set;} 		public Dictionary<int, bool> ReadyPlayers = new Dictionary<int, bool>();		public Dictionary<FightTeam, int> TotalTeamHealth {get;set;}		public List<FightMove> Moves {get;set;}		public Dictionary<int, Dictionary<int, ExchangeProfile>> MoveExchanges {get;set;}		public int ExchangeCount {get;set;}		public delegate Fight Factory(Guid fightId, string creator, FightType type, int teamSize, float timeout);

		protected static ILogger Log = LogManager.GetCurrentClassLogger();

		public Fight(Guid fightId, string creator, FightType type, int teamSize, float timeout, bool sanguinary, bool botsWelcome = true)		{			CharFightData = new Dictionary<CCharacter, CurrentFightCharData>();			TeamBlue = new Dictionary<int, CCharacter>();			TeamRed = new Dictionary<int, CCharacter>();			TotalTeamHealth = new Dictionary<FightTeam, int>() { { FightTeam.Red, 0 }, { FightTeam.Blue, 0 }, };			Moves = new List<FightMove>();			MoveExchanges = new Dictionary<int, Dictionary<int, ExchangeProfile>>();			ExchangeCount = 0;			_fightId = fightId;			Creator = creator;			State = FightState.QUEUE;			Type = type;			TeamSize = teamSize;			Timeout = timeout;			Sanguinary = sanguinary;			BotsWelcome = botsWelcome;		}		public override string ToString()		{			return string.Format("{0} by {1} in {2} ({3}/{4})", FightId, Creator, State, getNumPlayers(), TeamSize*2);		}		public Dictionary<int, CPlayerInstance> Players		{			get			{				return getAllParticipants().OfType<CPlayerInstance>().ToDictionary(player => player.ObjectId);			}			set{}		}		private Dictionary<int, CBotInstance> Bots		{			get			{				return getAllParticipants().OfType<CBotInstance>().ToDictionary(bot => bot.ObjectId);			}			set {}		} 		#region QUEUE OPERATIONS		public bool addPlayer(CCharacter player) //TODO: player has to choose team, when GROUP fight		{			bool result = false;			if (getNumPlayers() == TeamSize*2) return result;			if (getNumPlayers() < TeamSize * 2 && State == FightState.QUEUE) //if both teams are not full, add to any			{				if (TeamRed.Count < TeamSize && TeamBlue.Count < TeamSize)				{					result = addPlayer(new Random().Next(1, 3), player);				}				else				{					if (TeamRed.Count < TeamSize) //if red not full ad to red					{						result = addPlayer(1, player);					}					else					{						result = addPlayer(2, player); //if red full - add to blue					}				}			}			return result;		}		public bool addPlayer(int team, CCharacter player)		{			//TODO:redo this ugly temp logic. create a Team class			var targetTeam = (team != 1) ? TeamBlue : TeamRed;			if (player.CurrentFight == null)			{				targetTeam.Add(player.ObjectId, player);				player.CurrentFight = this;				CharFightData.Add(player, new CurrentFightCharData() { Team = (team != 1) ? FightTeam.Blue : FightTeam.Red, TotalDamage = 0, WLT = null });				TotalTeamHealth[(team != 1) ? FightTeam.Blue : FightTeam.Red] += (int)player.Stats.GetStat(new MaxHealth());				//Log.DebugFormat("({0} added to {1}) blue team HP: {2} / red team HP: {3}", player.Name,(team != 1) ? FightTeam.Blue : FightTeam.Red, TotalTeamHealth[FightTeam.Blue], TotalTeamHealth[FightTeam.Red]);				shouldSendReadyPrompts();				if (player is CBotInstance)				{					SetPlayerReady(player.ObjectId, true);				}

				var fightParticipants = new FightQueueParticipants(this);
				var pulledQueues = new PulledQueues(Players.Values.FirstOrDefault().FightManager);
				foreach (var cplayer in Players.Values)
				{
					cplayer.SendPacket(pulledQueues);
					cplayer.SendPacket(fightParticipants);
				}
				return true;			}			return false;		}		private void shouldSendReadyPrompts()		{			if(getNumPlayers() == TeamSize*2)			{				foreach (var p in Players.Values)				{					p.SendPacket(new ReadyPrompt());				}			}		}		public void SetPlayerReady(int objId, bool status)		{			if(!ReadyPlayers.ContainsKey(objId))			{				ReadyPlayers.Add(objId, status);			}			CheckIfAllReady();		}		private void CheckIfAllReady()		{			if(ReadyPlayers.Count == TeamSize*2)			{				State = FightState.ENGAGED;				foreach(var participant in getAllParticipants())				{					participant.SwitchCurrentFightTarget(); //set every players target (to a player they havent attacked yet					if (DEBUG) Log.DebugFormat("{0}'s fight data: {1}", participant.ToString(), CharFightData[participant]);					if (participant is CPlayerInstance)					{						((CPlayerInstance)participant).SendPacket(new StartFight()); //Load fight scene event for clients					}				}				ReadyPlayers = new Dictionary<int, bool>();			}		}		//try to remove from team blue, if fails remove from team red. return false if couldnt remove from any		public bool RemovePlayer(CCharacter player) //improve to a teamless argument		{			bool result = false;			if (TeamBlue.ContainsKey(player.ObjectId))			{				result = TeamBlue.Remove(player.ObjectId);			}			if (TeamRed.ContainsKey(player.ObjectId))			{				result = TeamRed.Remove(player.ObjectId);			}			if (result)			{				player.CurrentFight = null;				return result;			}			return result;		}
		#endregion
		#region QUERIES        public CCharacter getParticipant(int objectId)		{			CCharacter result;			if (!TeamRed.TryGetValue(objectId, out result))			{				TeamBlue.TryGetValue(objectId, out result);			}			if (result != null)			{				return result;			}			return null;		}		public int getNumPlayers()		{			return TeamRed.Count + TeamBlue.Count;		}		public List<CCharacter> getAllParticipants()		{			List<CCharacter> result;			result = TeamBlue.Values.ToList();			result.AddRange(TeamRed.Values.ToList());			return result;		}		public int getHighestLevel()		{			int result = 0;			foreach (var player in Players.Values)			{				var level = (int)player.Stats.GetStat<Level>();				result = (result < level) ? level : result;			}			return result;		}		public int getLowestLevel()		{
			int result = 999999;			foreach (var player in Players.Values)			{				var level = (int)player.Stats.GetStat<Level>();				result = (result > level) ? level : result;			}			return result;		}

		#endregion
		#region FIGHT OPERATIONS
		public void AddMoveSendPkg(CCharacter addingInstance, FightMove newMove)		{			if (IsDuplicateMove(newMove))			{				return;			}			Moves.Add(newMove);			if (DEBUG)			{				if (DEBUG) Log.DebugFormat("Adding move by: {0}.A: {1}, B: {2}-{3}, target: {4}", newMove.PeerObjectId, newMove.AttackSpot,					newMove.BlockSpots[0], newMove.BlockSpots[1], newMove.TargetObjectId);			}			var allMoves = new List<FightMove>(Moves);			foreach (var move in allMoves)			{				if (DEBUG)				{					Log.DebugFormat("Moves.move by: {0}.A: {1}, B: {2}-{3}, target: {4}", move.PeerObjectId, move.AttackSpot,						move.BlockSpots[0], move.BlockSpots[1], move.TargetObjectId);				}				if (newMove.TargetObjectId == move.PeerObjectId && newMove.PeerObjectId == move.TargetObjectId)				{					ProcessMoves(newMove, move);					Moves.Remove(newMove);					Moves.Remove(move);					if (State == FightState.FINISHED)					{						return;					}					DeadPlayerGarbageCollection();				}			}			addingInstance.SwitchCurrentFightTarget();			var cplayer = addingInstance as CPlayerInstance;			if (cplayer != null)			{				cplayer.SendPacket(new FightParticipants(cplayer, true));			}		}		private void DeadPlayerGarbageCollection()		{			foreach(var player in getAllParticipants())			{				//if dead delete all moves where this dead player is present either as ATTACKER or as DEFENDER				if(player.IsDead)				{					if(DEBUG)Log.DebugFormat("A dead player: {0}", player.ToString());					lock(this)					{						var moves = new List<FightMove>(Moves);						foreach(var move in moves)						{							if(move.PeerObjectId == player.ObjectId || move.TargetObjectId == player.ObjectId)							{								if(DEBUG)Log.DebugFormat("moves involving this dead player: {0}", move);								Moves.Remove(move);							}						}					}				}				//if not dead - check if target is dead and assign new if dead				else				{					if(DEBUG)Log.DebugFormat("checking if player {0} has a dead target", player.ToString());					var playersTarget = player.Target as CCharacter;					if(DEBUG)Log.DebugFormat("got a target as CCharacter : {0}", (playersTarget != null));					if(playersTarget != null)					{						if(playersTarget.IsDead)						{							if(DEBUG)Log.DebugFormat("target is dead, sending player to target switcher");							player.SwitchCurrentFightTarget();							var cplayer = player as CPlayerInstance;							if (cplayer != null)							{								cplayer.SendPacket(new FightParticipants(cplayer, true));							}						}					}				} 			}		}		private bool IsDuplicateMove(FightMove move)		{			foreach(var m in Moves)			{				if(m.PeerObjectId == move.PeerObjectId && m.TargetObjectId == move.TargetObjectId)				{					return true;				}			}			return false;		}		public void ProcessMoves(FightMove playerMove, FightMove opponentMove)		{			var instance = getParticipant(playerMove.PeerObjectId);			var opponent = getParticipant(opponentMove.PeerObjectId);			var playerOutcome = Util.CheckAhitB(playerMove, opponentMove); //check if player A's attack has succeeded(Hit) or was blocked(Block)			var opponentOutcome = Util.CheckAhitB(opponentMove, playerMove); // -- || --			var result = new Dictionary<int, ExchangeProfile>(); //client fight log item			var opponentTeamHealth = ProcessOutcome(instance, opponent, playerOutcome, result); //do the actual hit/block, with all the stats			var playerTeamHealth = ProcessOutcome(opponent, instance, opponentOutcome, result);//do the actual hit/block, with all the stats			updateTotalTeamHealth(getPlayerTeam(opponent), -opponentTeamHealth);			updateTotalTeamHealth(getPlayerTeam(instance), -playerTeamHealth);			MoveExchanges.Add(++ExchangeCount, result); //add exchange result to the Fight log			if(DEBUG) Log.DebugFormat("teamA({0}) hp: {1}/ teamB({2}) hp: {3}", instance.Name, opponentTeamHealth, opponent.Name, playerTeamHealth);			foreach(var player in Players.Values)			{				player.SendPacket(new FightUpdate(player, result));			}			checkToFinish(getTotalTeamHealth(FightTeam.Red), getTotalTeamHealth(FightTeam.Blue));		}		public int ProcessOutcome(CCharacter attacker, CCharacter target, MoveOutcome outcome, Dictionary<int, ExchangeProfile> result)		{			var rgen = new Random();			var moveLog = new ExchangeProfile(attacker.ObjectId);			var attackerStats = attacker.Stats;			var minDamage = (int)attackerStats.GetStat(new MinDamage());			var maxDamage = (int)attackerStats.GetStat(new MaxDamage());			var damage = rgen.Next(minDamage, maxDamage+1);			switch(outcome)			{				case(MoveOutcome.Hit):					//HIT CHANCE VS TARGET'S DODGE CHANCE					if (attackerStats.GetStat(new HitChance(), target) > rgen.Next(0, 100))					{						//CRIT CHANCE VS TARGET'S ANTI-CRIT CHANCE, if fails then HIT						damage = attackerStats.GetStat(new CriticalHitChance(), target) > rgen.Next(0, 100)							? applyOutcome(attacker, target, MoveOutcome.Crit, damage, moveLog)							: applyOutcome(attacker, target, MoveOutcome.Hit, damage, moveLog);					}					else					{						//IF HIT 'MISSES' ITS A DODGE						damage = applyOutcome(attacker, target, MoveOutcome.Dodge, damage, moveLog);					}					break;				case(MoveOutcome.Block):					damage = applyOutcome(attacker, target, 						(attacker.Stats.GetStat(new CriticalHitChance(), target))*0.2f > rgen.Next(0,100) ? MoveOutcome.BlockCrit : MoveOutcome.Block,						damage, moveLog);					break;				default:					if (DEBUG) Log.DebugFormat("SendMoveHandler.ProcessOutcome - moveoutcome unknown");					break;			}			result.Add(moveLog.objectId, moveLog);			return damage;		}		private int applyOutcome(CCharacter attacker, CCharacter target, MoveOutcome outcome, int damage, ExchangeProfile moveLog)		{			switch(outcome)			{				case (MoveOutcome.Crit):		damage += (int)attacker.Stats.GetStat(new CriticalDamage());												break;				case (MoveOutcome.Dodge):		damage = 0;												break;				case (MoveOutcome.BlockCrit):	damage += (int)(attacker.Stats.GetStat(new CriticalDamage()) * 1.15f); //extra damage for critting through block												break;				case (MoveOutcome.Block):		damage = 0;												break;				default: break;			}			moveLog.outcome = outcome;			moveLog.damage = damage;			moveLog.totalDamage = updateTotalDamage(attacker, damage); //bonus damage is accrued before potential overkill clamp			//All time stats: damage++, crit++, critdamage++, hit++, dodge++, blockcrit++, block++			return target.Stats.ApplyDamage(damage); //clamps to actual damage dealt on overkill then inflicts and returns its value		}		private void checkToFinish(int teamRedHP, int teamBlueHP)		{			//No team has lost yet			if (teamRedHP > 0 && teamBlueHP > 0) return;			//TIE			if (teamRedHP <= 0 && teamBlueHP <= 0)			{				foreach (var player in getAllParticipants())				{					setPlayerFightWLT(player, FightWinLossTie.Tie);				}			}			else //NO TIE			{				var teamThatWon = teamRedHP <= 0 ? FightTeam.Blue : FightTeam.Red;				setAllParticipantsWLT(teamThatWon);			}			finishFight();		}		private void setAllParticipantsWLT(FightTeam teamThatWon)		{			var tempPlayers = getAllParticipants().ToDictionary(x => x.ObjectId, x => x);			foreach (var wonPlayer in getAllParticipants().Where(t => getPlayerTeam(t) == teamThatWon))			{				setPlayerFightWLT(wonPlayer, FightWinLossTie.Win);				tempPlayers.Remove(wonPlayer.ObjectId);			}			foreach (var lostPlayer in tempPlayers)			{				setPlayerFightWLT(lostPlayer.Value, FightWinLossTie.Loss);			}		}		private int updateTotalDamage(CCharacter obj, int value)		{			var info = CharFightData[obj];			info.TotalDamage += value;			CharFightData[obj] = info;			return info.TotalDamage;		}		private void updateTotalTeamHealth(FightTeam team, int damage)		{			var health = TotalTeamHealth[team];			health += damage;			Log.DebugFormat("TeamHPupdate: current: {0}, dmg to be inflicted: {1}, new: {2}", TotalTeamHealth[team], damage, health);			TotalTeamHealth[team] = health;		}		private int getTotalTeamHealth(FightTeam team)		{			return TotalTeamHealth[team];		}		private FightTeam getPlayerTeam(CCharacter player)		{			return CharFightData[player].Team;		}		private void setPlayerFightWLT(CCharacter player, FightWinLossTie result)		{			var fightData = CharFightData[player];			fightData.WLT = result;			CharFightData[player] = fightData;		}		private FightWinLossTie getPlayerFightWLT(CCharacter player)		{			return CharFightData[player].WLT.Value;		}		#endregion		#region FINISHING FIGHT		public void finishFight()		{			if (DEBUG) Log.DebugFormat("finishing fight");			State = FightState.FINISHED;			foreach(var player in Players.Values)			{				var reward = CalculateAndGrantRewards(player);				UpdateGenStats(player, reward);				player.SendPacket(new FinishFight(reward));				player.CurrentFight = null;				player.Resurrect();			}			//store fight history in archive		}		private Rewards CalculateAndGrantRewards(CPlayerInstance instance)		{			var reward = new Rewards();			if (DEBUG) Log.DebugFormat("{0} has ended the fight with a {1}", instance.Name, instance.CurrentFight.CharFightData[instance].WLT);			switch(getPlayerFightWLT(instance))			{				case(FightWinLossTie.Win):					reward.Experience = (int)(CharFightData[instance].TotalDamage * (instance.Stats.GetStat<Level>()*1.15f));					reward.Gold = (int)(new Random().Next(0,2) < 1 ? (new Random().Next(1,14) * instance.Stats.GetStat<Level>()) : 0);					reward.WLT = FightWinLossTie.Win;					reward.TotalDamage = CharFightData[instance].TotalDamage;					if(CharFightData[instance].Injuried) 					{ 						/*instance.ApplyInjury(time);*/ 						reward.Injury = true;					}					if (DEBUG) Log.DebugFormat("WON: Reward:{0} exp:{1}, gold:{2}, win:{3}, dmg:{4}", instance.ObjectId, reward.Experience, reward.Gold, reward.WLT, reward.TotalDamage);					break;				case(FightWinLossTie.Loss): //no rewards				case(FightWinLossTie.Tie):					reward.Experience = (int)(CharFightData[instance].TotalDamage * 0.5f);					reward.Gold = (int)(new Random().Next(0,1000) < 1 ? (3*instance.Stats.GetStat<Level>()) : 0);					reward.WLT = (instance.CurrentFight.CharFightData[instance].WLT == FightWinLossTie.Loss) ? FightWinLossTie.Loss : FightWinLossTie.Tie;					reward.TotalDamage = CharFightData[instance].TotalDamage;					if(CharFightData[instance].Injuried) 						{ 						/*instance.ApplyInjury(time);*/ 						reward.Injury = true;					}					if (DEBUG) Log.DebugFormat("{0}: Reward:{1} exp:{2}, gold:{3}, win:{4}, dmg:{5}",instance.CurrentFight.CharFightData[instance].WLT, instance.ObjectId, reward.Experience, reward.Gold, reward.WLT, reward.TotalDamage);					break;			}			return reward;		}		private void UpdateGenStats(CPlayerInstance player, Rewards reward)		{			var genStats = player.GenStats;			genStats.AddExperience((int)reward.Experience);			genStats.Gold += (int)reward.Gold;			genStats.Battles++;			switch(CharFightData[player].WLT)			{				case(FightWinLossTie.Win):	genStats.Win++;											break;				case(FightWinLossTie.Loss): genStats.Loss++;											break;				case(FightWinLossTie.Tie):	genStats.Tie++;											break;			}		}		#endregion		#region CLIENT PACKAGE INFO		public FightQueueListItem GetQueueInfo()		{			var result = new FightQueueListItem()												{													FightId = this._fightId.ToString(),													Type = this.Type,													Creator = this.Creator,													Blue = TeamBlue.Select(p => p.Value.Name).ToList(),													Red = TeamRed.Select(p => p.Value.Name).ToList(),													Timeout = this.Timeout,													TeamSize = this.TeamSize,													Sanguinary = this.Sanguinary,												};			return result;		}		#endregion	}}