﻿using RegionServer.Model.Interfaces;

		protected static ILogger Log = LogManager.GetCurrentClassLogger();

		public Fight(Guid fightId, string creator, FightType type, int teamSize, float timeout, bool sanguinary, bool botsWelcome = true)
		    }

				var fightParticipants = new FightQueueParticipants(this);
			    var pulledQueues = new PulledQueues(getPlayers.Values.FirstOrDefault().FightManager);
				foreach (var cplayer in getPlayers.Values)
				{
					cplayer.SendPacket(pulledQueues);
					cplayer.SendPacket(fightParticipants);
				}

		#endregion
		#region QUERIES

        public bool isFull
        {
            get
            {
                return getNumParticipants() >= TeamSize * 2;
            }
        }

        public List<CCharacter> getAllParticipants()
			int result = 999999;

		#endregion
		#region FIGHT OPERATIONS
		public void AddMoveSendPkg(CCharacter addingInstance, FightMove newMove)

            if(matchingMove != null)

                MoveExchanges.Add(++ExchangeCount, resultObj); //add exchange result to the Fight log

                foreach (var player in getPlayers.Values)
                {
                    player.SendPacket(new FightUpdate(player, resultObj));
                }

            DeadPlayerGarbageCollection(); //cleares all dead players' targets & dead targets

            addingInstance.SwitchCurrentFightTarget();

            notifyBots(newMove);

            var cplayer = addingInstance as CPlayerInstance;

                        FightMove trashCan;
        #endregion
        #region FINISHING FIGHT
        {
            //No team has lost yet
            if (teamRedHP > 0 && teamBlueHP > 0) return FightState.ENGAGED;

            //TIE
            if (teamRedHP <= 0 && teamBlueHP <= 0)
            {
                foreach (var player in getAllParticipants())
                {
                    setPlayerFightWLT(player, FightWinLossTie.Tie);
                }
            }
            else //NO TIE
            {
                var teamThatWon = teamRedHP <= 0 ? FightTeam.Blue : FightTeam.Red;
                setAllParticipantsWLT(teamThatWon);
            }

        private void finishFight()
                player.GenStats.postFightUpdate((FightWinLossTie)CharFightData[player].WLT, reward);