using RegionServer.Model.Interfaces;using ComplexServerCommon.Enums;using System.Collections.Generic;using System;using System.Collections.Concurrent;using ComplexServerCommon.MessageObjects;using System.Linq;using System.Security.Cryptography.X509Certificates;using RegionServer.Model.ServerEvents;using ExitGames.Logging;using RegionServer.Model.CharacterDatas;using RegionServer.Model.DataKeepers;using RegionServer.Model.Stats;using RegionServer.Model.Stats.BaseStats;using Remotion.Linq.Collections;namespace RegionServer.Model.Fighting{	public class Fight : IFight	{		private readonly bool DEBUG = true;		private readonly Guid _fightId;		public Guid FightId {get { return _fightId;} set{} }		public string Creator {get;set;}		public FightState fightState {get; set;}		public FightType Type {get; set;}		public int TeamSize {get; set;}		public float Timeout {get; set;}		public bool Sanguinary {get; set;}		public bool BotsWelcome { get; set; }	    public Position FightLocation { get; set; }	    public string Winner { get; set; }	    public Dictionary<CCharacter, CurrentFightCharData> CharFightData {get;set;}		public Dictionary<int, CCharacter> TeamBlue {get; set;} //int as objId		public Dictionary<int, CCharacter> TeamRed {get; set;} 		public Dictionary<int, bool> ReadyPlayers = new Dictionary<int, bool>();        public Dictionary<FightTeam, double> TeamElos { get; set; } 		public Dictionary<FightTeam, int> TotalTeamHealth {get;set;}		public ConcurrentDictionary<int, FightMove> Moves {get;set;}		public Dictionary<int, Dictionary<int, ExchangeProfile>> MoveExchanges {get;set;}		public int ExchangeCount {get;set;}		public delegate Fight Factory(FightManager fm, Guid fightId, string creator, FightType type, int teamSize, float timeout, bool sanguinary);	    private FightInformation fightPersistence;	    private readonly FightManager _fightManager;

		protected static ILogger Log = LogManager.GetCurrentClassLogger();

		public Fight(FightManager fm, Guid fightId, string creator, FightType type, int teamSize, float timeout, bool sanguinary, bool botsWelcome = true)		{			CharFightData = new Dictionary<CCharacter, CurrentFightCharData>();			TeamBlue = new Dictionary<int, CCharacter>();			TeamRed = new Dictionary<int, CCharacter>();			TotalTeamHealth = new Dictionary<FightTeam, int>() { {FightTeam.Red, 0}, {FightTeam.Blue, 0}, };			Moves = new ConcurrentDictionary<int,FightMove>();			MoveExchanges = new Dictionary<int, Dictionary<int, ExchangeProfile>>();            TeamElos = new Dictionary<FightTeam, double>();			ExchangeCount = 0;			_fightId = fightId;			Creator = creator;			fightState = FightState.QUEUE;			Type = type;			TeamSize = teamSize;			Timeout = timeout;			Sanguinary = sanguinary;			BotsWelcome = botsWelcome;            FightLocation = new Position(LocationType.CITY, LocationType.FIGHT);            fightPersistence = new FightInformation(this);		    _fightManager = fm;		}		public override string ToString()		{			return string.Format("{0} by {1} in {2} ({3}/{4})", FightId, Creator, fightState, getNumParticipants(), TeamSize*2);		}		public Dictionary<int, CPlayerInstance> getPlayers		{			get			{                var result = getAllParticipants().OfType<CPlayerInstance>().ToDictionary(player => player.ObjectId);			    return result;			}			set{}		}		public Dictionary<int, CBotInstance> getBots		{		    get		    {
                var result = getAllParticipants().OfType<CBotInstance>().ToDictionary(bot => bot.ObjectId);
                return result;
            }			set {}		} 		#region QUEUE OPERATIONS		public bool addPlayer(CCharacter player) //TODO: player has to choose team, when GROUP fight		{			bool result = false;			if (getNumParticipants() == TeamSize*2) return result;			if (getNumParticipants() < TeamSize * 2 && fightState == FightState.QUEUE) //if both teams are not full, add to any			{				if (TeamRed.Count < TeamSize && TeamBlue.Count < TeamSize)				{					result = addPlayer(new Random().Next(1, 3), player);				}				else				{					if (TeamRed.Count < TeamSize) //if red not full ad to red					{						result = addPlayer(1, player);					}					else					{						result = addPlayer(2, player); //if red full - add to blue					}				}			}			return result;		}		public bool addPlayer(int team, CCharacter player)		{			//TODO: redo this ugly temp logic. create a Team class			var targetTeam = (team != 1) ? TeamBlue : TeamRed;			if (player.CurrentFight == null)			{				targetTeam.Add(player.ObjectId, player);				player.CurrentFight = this;				CharFightData.Add(player, new CurrentFightCharData() { Team = (team != 1) ? FightTeam.Blue : FightTeam.Red, TotalDamage = 0, WLT = null });				TotalTeamHealth[(team != 1) ? FightTeam.Blue : FightTeam.Red] += (int)player.Stats.GetStat(new CurrHealth());				Log.DebugFormat("({0} added to {1}) blue team HP: {2} / red team HP: {3}", player.ToString(),(team != 1) ? FightTeam.Blue : FightTeam.Red, TotalTeamHealth[FightTeam.Blue], TotalTeamHealth[FightTeam.Red]);				shouldSendReadyPrompts();				if (player is CBotInstance)				{					SetPlayerReady(player.ObjectId, true);				}

				var fightParticipants = new FightQueueParticipantsPacket(this);
			    var pulledQueues = new PulledQueuesPacket(getPlayers.Values.FirstOrDefault().FightManager);
				foreach (var cplayer in getPlayers.Values)
				{
					cplayer.SendPacket(pulledQueues);
					cplayer.SendPacket(fightParticipants);
				}
				return true;			}			return false;		}		private void shouldSendReadyPrompts()		{			if(getNumParticipants() == TeamSize*2)			{				foreach (var p in getPlayers.Values)				{					p.SendPacket(new ReadyPromptPacket());				}			}		}		public void SetPlayerReady(int objId, bool status)		{			if(!ReadyPlayers.ContainsKey(objId))			{				ReadyPlayers.Add(objId, status);			}			CheckIfAllReady();		}	    private void CheckIfAllReady()	    {	        if (ReadyPlayers.Count == TeamSize*2)	        {                StartFight();	        }	    }	    private void StartFight()	    {
            fightState = FightState.ENGAGED;

            foreach (var participant in getAllParticipants())
            {
                participant.SwitchCurrentFightTarget(); //set every players target (to a player they havent attacked yet
                var player = participant as CPlayerInstance;
                if (player != null)
                {
                    player.SendPacket(new StartFightPacket()); //Load fight scene event for clients
                }
            }
            TeamElos.Add(FightTeam.Blue, getTeamElo(FightTeam.Blue));
            TeamElos.Add(FightTeam.Red, getTeamElo(FightTeam.Red));

            ReadyPlayers = new Dictionary<int, bool>();
            fightPersistence.FightStarted();
        }	    private double getTeamElo(FightTeam team)	    {	        double result = 0;	        int count = 0;	        foreach(var player in getAllParticipants().Where(p => CharFightData[p].Team == team))	        {	            result += player.GetCharData<EloKeeper>().GetElo();	            count++;	        }	        return (result/count);	    }
		//try to remove from team blue, if fails remove from team red. return false if couldnt remove from any		public bool RemovePlayer(CCharacter player) //improve to a teamless argument		{			bool result = false;			if (TeamBlue.ContainsKey(player.ObjectId))			{				result = TeamBlue.Remove(player.ObjectId);			}			if (TeamRed.ContainsKey(player.ObjectId))			{				result = TeamRed.Remove(player.ObjectId);			}			if (result)			{				player.CurrentFight = null;				return result;			}			return result;		}
		#endregion
		#region QUERIES        public CCharacter getParticipant(int objectId)		{			CCharacter result;			if (!TeamRed.TryGetValue(objectId, out result))			{				TeamBlue.TryGetValue(objectId, out result);			}			if (result != null)			{				return result;			}			return null;		}		public int getNumParticipants()		{			return TeamRed.Count + TeamBlue.Count;		}	    public int getNumPlayers()	    {	        return getPlayers.Values.Count;	    }	    public int getNumBots()	    {	        return getBots.Count;	    }	    public bool isBotsBothSides()	    {	        return (TeamRed.Values.Any(entity => entity.isNPC) && TeamBlue.Values.Any(entity => entity.isNPC));	    }

        public bool isFull
        {
            get
            {
                return getNumParticipants() >= TeamSize * 2;
            }
        }	    public bool hasMovesAgainstAll(CCharacter character)	    {	        var aliveEnemyIds = getAllParticipants().Where(x => getPlayerTeam(x) != getPlayerTeam(character)	                                                            && !x.IsDead).Select(x => x.ObjectId).ToList();	        bool result = false;            foreach(var id in aliveEnemyIds)            {                result = Moves.Values.Any(x => x.PeerObjectId == character.ObjectId && x.TargetObjectId == id);            }	        return result;	    }

        public List<CCharacter> getAllParticipants()		{			List<CCharacter> result;			result = TeamBlue.Values.ToList();			result.AddRange(TeamRed.Values.ToList());			return result;		}		public int getHighestLevel()		{			int result = 0;			foreach (var player in getPlayers.Values)			{				var level = (int)player.Stats.GetStat<Level>();				result = (result < level) ? level : result;			}			return result;		}		public int getLowestLevel()		{
			int result = 999999;			foreach (var player in getPlayers.Values)			{				var level = (int)player.Stats.GetStat<Level>();				result = (result > level) ? level : result;			}			return result;		}

		#endregion
		#region FIGHT OPERATIONS
		public void AddMoveSendPkg(CCharacter addingInstance, FightMove newMove)		{			if (isDuplicateMove(newMove))			{                if(DEBUG)Log.DebugFormat("Fight - AddMoveSendPkg - duplicate move by ({0}) was refused", newMove.PeerObjectId);				return;			}			Moves.TryAdd(newMove.PeerObjectId, newMove);			if (true) Log.DebugFormat("Adding move by: {0}.A: {1}, B: {2}-{3}, target: {4}", newMove.PeerObjectId, newMove.AttackSpot, newMove.BlockSpots[0], newMove.BlockSpots[1], newMove.TargetObjectId);            var matchingMove = Moves.FirstOrDefault(m => m.Value.PeerObjectId == newMove.TargetObjectId && m.Value.TargetObjectId == newMove.PeerObjectId).Value;

            if(matchingMove != null)			{				if (true) Log.DebugFormat("Moves.move by: {0}.A: {1}, B: {2}-{3}, target: {4}", matchingMove.PeerObjectId, matchingMove.AttackSpot, matchingMove.BlockSpots[0], matchingMove.BlockSpots[1], matchingMove.TargetObjectId);
                Process(newMove, matchingMove);
            }

            DeadPlayerGarbageCollection(); //cleares all dead players' targets & dead targets

            addingInstance.SwitchCurrentFightTarget();

            //notifyBots(newMove);

            var cplayer = addingInstance as CPlayerInstance;			if (cplayer != null)			{				cplayer.SendPacket(new FightParticipantsPacket(cplayer, true));			}		}	    private void Process(FightMove newMove, FightMove matchingMove)	    {
            var processor = new MoveProcessor(this);
            var resultObj = processor.ProcessMoves(newMove, matchingMove);
            MoveExchanges.Add(++ExchangeCount, resultObj); //add exchange result to the Fight log
            foreach (var obj in resultObj.Values)
            {
                Log.DebugFormat("Attacker: {0} / Outcome: {1} / Damage: {2} / Total Damage: {3}", obj.objectId, obj.outcome, obj.damage, obj.totalDamage);
            }

            foreach (var player in getPlayers.Values)
            {
                player.SendPacket(new FightUpdatePacket(player, resultObj));
            }

            fightState = checkForFinish(getTotalTeamHealth(FightTeam.Red), getTotalTeamHealth(FightTeam.Blue));

            if (fightState == FightState.FINISHED)
            {
                finishFight();
                return;
            }

            FightMove trashCan;
            Moves.TryRemove(newMove.PeerObjectId, out trashCan);
            Moves.TryRemove(matchingMove.PeerObjectId, out trashCan);
        }
		private void DeadPlayerGarbageCollection()		{			foreach(var player in getAllParticipants())			{				//if dead delete all moves where this dead player is present either as ATTACKER or as DEFENDER				if(player.IsDead)				{					if(false)Log.DebugFormat("A dead player: {0}", player.ToString());                    var deadPlayerMoves = Moves.Where(m => m.Value.PeerObjectId == player.ObjectId || m.Value.TargetObjectId == player.ObjectId).ToList();

                    FightMove trashCan;					foreach(var move in deadPlayerMoves)					{						if(true)Log.DebugFormat("moves involving this dead player: {0}", move);						Moves.TryRemove(move.Key, out trashCan);					}				}				//if not dead - check if target is dead and assign new if dead				else				{				    var playersTarget = player.Target as CCharacter;				    if (playersTarget == null || !playersTarget.IsDead) continue;                    Log.DebugFormat("CHANGING TARGET WHERE IT SHOULD NOT BE THE CASE?");				    player.SwitchCurrentFightTarget();				    var cplayer = player as CPlayerInstance;				    if (cplayer != null)				    {				        cplayer.SendPacket(new FightParticipantsPacket(cplayer, true)); //true means send only the target update. perhaps a new serverpacket would be better				    }				} 			}		}		private bool isDuplicateMove(FightMove move)		{		    foreach (var movez in Moves.Values)		    {		        Log.DebugFormat("checking moves (in dupe method): {0} vs {1}", movez.PeerObjectId, movez.TargetObjectId);		    }		    return Moves.Values.Any(m => m.PeerObjectId == move.PeerObjectId && m.TargetObjectId == move.TargetObjectId);		}		private void setAllParticipantsWLT(FightTeam teamThatWon)		{			var tempPlayers = getAllParticipants().ToDictionary(x => x.ObjectId, x => x);			foreach (var wonPlayer in getAllParticipants().Where(t => getPlayerTeam(t) == teamThatWon))			{				setPlayerFightWLT(wonPlayer, FightWinLossTie.Win);				tempPlayers.Remove(wonPlayer.ObjectId);			}			foreach (var lostPlayer in tempPlayers)			{				setPlayerFightWLT(lostPlayer.Value, FightWinLossTie.Loss);			}		}		public int updateTotalDamage(CCharacter obj, int value)		{			var info = CharFightData[obj];			info.TotalDamage += value;			CharFightData[obj] = info;			return info.TotalDamage;		}		public void updateTotalTeamHealth(FightTeam team, int damage)		{			var health = TotalTeamHealth[team];			health += damage;			Log.DebugFormat("TeamHPupdate: current: {0}, dmg to be inflicted: {1}, new: {2}", TotalTeamHealth[team], -damage, health);			TotalTeamHealth[team] = health;		}		public int getTotalTeamHealth(FightTeam team)		{			return TotalTeamHealth[team];		}		public FightTeam getPlayerTeam(CCharacter player)		{			return CharFightData[player].Team;		}		private void setPlayerFightWLT(CCharacter player, FightWinLossTie result)		{			var fightData = CharFightData[player];			fightData.WLT = result;			CharFightData[player] = fightData;		}		private FightWinLossTie getPlayerFightWLT(CCharacter player)		{			return CharFightData[player].WLT.Value;		}
        #endregion
        #region FINISHING FIGHT        private FightState checkForFinish(int teamRedHP, int teamBlueHP)
        {
            //No team has lost yet
            if (teamRedHP > 0 && teamBlueHP > 0) return FightState.ENGAGED;

            //TIE
            if (teamRedHP <= 0 && teamBlueHP <= 0)
            {
                foreach (var player in getAllParticipants())
                {
                    setPlayerFightWLT(player, FightWinLossTie.Tie);
                }                Winner = "Tie";            }
            else //NO TIE
            {
                var teamThatWon = teamRedHP <= 0 ? FightTeam.Blue : FightTeam.Red;                Winner = teamThatWon == FightTeam.Blue ? "Blue" : "Red";
                setAllParticipantsWLT(teamThatWon);
            }            return FightState.FINISHED;        }

        private void finishFight()		{			if (DEBUG) Log.DebugFormat("finishing fight");			foreach(var player in getPlayers.Values)			{			    var reward = calculateRewards(player);			    var charFightData = CharFightData[player];
                player.GetCharData<GeneralStats>().postFightUpdate(charFightData.WLT.Value, reward);			    player.GetCharData<EloKeeper>().postFightUpdate(			        charFightData.WLT.Value, TeamElos[charFightData.Team],			        TeamElos[(charFightData.Team == FightTeam.Blue ? FightTeam.Red : FightTeam.Blue)]);                Log.DebugFormat("Char: {0} new elo is: {1}", player.Name, player.GetCharData<EloKeeper>().GetElo());				player.SendPacket(new FinishFightPacket(reward));				player.CurrentFight = null;				player.Resurrect();			}			//store fight history in archive            fightPersistence.store();            _fightManager.RemoveFight(this);		}		private Rewards calculateRewards(CPlayerInstance instance)		{			var reward = new Rewards();			if (false) Log.DebugFormat("{0} has ended the fight with a {1}", instance.Name, instance.CurrentFight.CharFightData[instance].WLT);			switch(getPlayerFightWLT(instance))			{				case(FightWinLossTie.Win):					reward.Experience = (int)(CharFightData[instance].TotalDamage * (instance.Stats.GetStat<Level>()*0.15f));					reward.Gold = (int)(new Random().Next(0,2) < 1 ? (new Random().Next(1,14) * instance.Stats.GetStat<Level>()) : 0);					reward.WLT = FightWinLossTie.Win;					reward.TotalDamage = CharFightData[instance].TotalDamage;					if(CharFightData[instance].Injuried) 					{ 						/*instance.ApplyInjury(time);*/ 						reward.Injury = true;					}					if (false) Log.DebugFormat("WON: Reward:{0} exp:{1}, gold:{2}, win:{3}, dmg:{4}", instance.ObjectId, reward.Experience, reward.Gold, reward.WLT, reward.TotalDamage);					break;				case(FightWinLossTie.Loss): //no rewards				case(FightWinLossTie.Tie):					reward.Experience = (int)(CharFightData[instance].TotalDamage * 0.5f);					reward.Gold = (int)(new Random().Next(0,1000) < 1 ? (3*instance.Stats.GetStat<Level>()) : 0);					reward.WLT = (instance.CurrentFight.CharFightData[instance].WLT == FightWinLossTie.Loss) ? FightWinLossTie.Loss : FightWinLossTie.Tie;					reward.TotalDamage = CharFightData[instance].TotalDamage;					if(CharFightData[instance].Injuried) 						{ 						/*instance.ApplyInjury(time);*/ 						reward.Injury = true;					}					if (false) Log.DebugFormat("{0}: Reward:{1} exp:{2}, gold:{3}, win:{4}, dmg:{5}",instance.CurrentFight.CharFightData[instance].WLT, instance.ObjectId, reward.Experience, reward.Gold, reward.WLT, reward.TotalDamage);					break;			}			return reward;		}		#endregion		#region CLIENT PACKAGE INFO		public FightQueueListItem GetQueueInfo()		{			var result = new FightQueueListItem()									{										FightId = this._fightId.ToString(),										Type = this.Type,										Creator = this.Creator,										Blue = TeamBlue.Select(p => p.Value.Name).ToList(),										Red = TeamRed.Select(p => p.Value.Name).ToList(),										Timeout = this.Timeout,										TeamSize = this.TeamSize,										Sanguinary = this.Sanguinary,									};			return result;		}		#endregion	}}