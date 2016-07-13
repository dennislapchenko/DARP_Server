using ComplexServerCommon;
using System.Collections.Generic;
using ComplexServerCommon.MessageObjects;
using ComplexServerCommon.Enums;
using RegionServer.Model.Fighting;

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
				var stats = player.Stats.GetMainStatsForEnemy();
				TeamRedInfos.Add(new CharInfo()
											{
                                                ObjectId = player.ObjectId,
												Name = player.Name,
												GenStats = player.GenStats,
												Stats = player.Stats.GetMainStatsForEnemy(),
											});
				Log.DebugFormat("FQP {0} added to team red (client packet)", player.Name);
			}
			foreach(var player in fight.TeamBlue.Values)
			{
				TeamBlueInfos.Add(new CharInfo()
											{
                                                ObjectId = player.ObjectId,
												Name = player.Name,
												GenStats = player.GenStats,
												Stats = player.Stats.GetMainStatsForEnemy(),
											});
				Log.DebugFormat("FQP {0} added to team blue (client packet)", player.Name);
			}
		}

		private void AddGroupPlayersInfo(Fight fight)
		{
			//NAME, LEVEL, CURRHEALTH/MAXHEALTH for each.
			foreach(var player in fight.TeamRed.Values)
			{
				TeamRedInfos.Add(new CharInfo()
											{
                                                ObjectId = player.ObjectId,
                                                Name = player.Name,
												Stats = player.Stats.GetHealthLevel(),
											});
				Log.DebugFormat("FQP {0} added to team red (client packet)", player.Name);
			}
			foreach(var player in fight.TeamBlue.Values)
			{
				TeamBlueInfos.Add(new CharInfo()
											{
                                                ObjectId = player.ObjectId,
												Name = player.Name,
												Stats = player.Stats.GetHealthLevel(),
											});
				Log.DebugFormat("FQP {0} added to team blue (client packet)", player.Name);
			}
		}
	}
}

