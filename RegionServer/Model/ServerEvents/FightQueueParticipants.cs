using ComplexServerCommon;
using System.Collections.Generic;
using ComplexServerCommon.MessageObjects;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using ComplexServerCommon.Enums;

namespace RegionServer.Model.ServerEvents
{
	public class FightQueueParticipants : ServerPacket
	{
		List<CharInfo> TeamRedInfos = new List<CharInfo>();
		List<CharInfo> TeamBlueInfos = new List<CharInfo>();
		public FightQueueParticipants(Fight fight) : base(ClientEventCode.ServerPacket, MessageSubCode.FightQueueParticipants)
		{
			//AddParameter(player.ObjectId, ClientParameterCode.ObjectId);
			AddPlayersInfos(fight);
		}

		private void AddPlayersInfos(Fight fight)
		{
			switch(fight.Type)
			{
				case(FightType.SINGLE):
					AddSinglePlayersInfo(fight);
					break;
				case(FightType.GROUP):
					AddGroupPlayersInfo(fight);
					break;
				default:
					break;
			}
			//TODO: allChars should be fightInfo
			var allChars = new FightCharsInfo();
			allChars.RedInfo = TeamRedInfos;
			allChars.BlueInfo = TeamBlueInfos;
			allChars.Type = fight.Type;
			//allChars.Sanguinity = fight.sanguinity;
			AddSerializedParameter(allChars, ClientParameterCode.Object, false);
		}

		private void AddSinglePlayersInfo(Fight fight)
		{
			foreach(var player in fight.TeamRed.Values)
			{
				var cplayer = player as CPlayerInstance;
				if(cplayer != null)
				{
					var stats = cplayer.Stats.GetMainStatsForEnemy();
					TeamRedInfos.Add(new CharInfo()
												{
													Name = cplayer.Name,
													GenStats = cplayer.GenStats,
													Stats = cplayer.Stats.GetMainStatsForEnemy(),
												});
					cplayer.Client.Log.DebugFormat("FQP {0} added to team red (client packet)", cplayer.Name);
				}
			}
			foreach(var player in fight.TeamBlue.Values)
			{
				var cplayer = player as CPlayerInstance;
				if(cplayer != null)
				{
					TeamBlueInfos.Add(new CharInfo()
												{
													Name = cplayer.Name,
													GenStats = cplayer.GenStats,
													Stats = cplayer.Stats.GetMainStatsForEnemy(),
												});
					cplayer.Client.Log.DebugFormat("FQP {0} added to team blue (client packet)", cplayer.Name);
				}
			}
		}

		private void AddGroupPlayersInfo(Fight fight)
		{
			//NAME, LEVEL, CURRHEALTH/MAXHEALTH for each.
			foreach(var player in fight.TeamRed.Values)
			{
				var cplayer = player as CPlayerInstance;
				if(cplayer != null)
				{
					TeamRedInfos.Add(new CharInfo()
												{
													Name = cplayer.Name,
													Stats = cplayer.Stats.GetHealthLevel(),
												});
					cplayer.Client.Log.DebugFormat("FQP {0} added to team red (client packet)", cplayer.Name);
				}
			}
			foreach(var player in fight.TeamBlue.Values)
			{
				var cplayer = player as CPlayerInstance;
				if(cplayer != null)
				{
					var stats = cplayer.Stats.GetHealthLevel();
					TeamBlueInfos.Add(new CharInfo()
												{
													Name = cplayer.Name,
													Stats = cplayer.Stats.GetHealthLevel(),
												});
					cplayer.Client.Log.DebugFormat("FQP {0} added to team blue (client packet)", cplayer.Name);
				}
			}
		}
	}
}

