using System.Collections.Generic;
using System.Linq;
using ComplexServerCommon;
using ComplexServerCommon.MessageObjects;
using RegionServer.Model.Fighting;
using RegionServer.Model.Items;

namespace RegionServer.Model.ServerEvents.FightEvents
{
	public class FightParticipantsPacket : ServerPacket
	{
		Dictionary<int, CharFightInfo> charsInfo;

		private CPlayerInstance _instance;
		public FightParticipantsPacket(CPlayerInstance player, bool onlyTarget) : base(ClientEventCode.ServerPacket, MessageSubCode.FightParticipants)
		{
			_instance = player;
			AddParameter(player.Target.ObjectId, ClientParameterCode.ObjectId);
			if(!onlyTarget)
			{
				AddInfo(player);
			}
		}
			
		private void AddInfo(CPlayerInstance player)
		{
			AddCharInfo(player.CurrentFight);
			var fightInfo = new FightInitInfo()
											{
												fightType = player.CurrentFight.Type, 
												sanguinary = player.CurrentFight.Sanguinary, 
												allChars = charsInfo,
											};
			AddSerializedParameter(fightInfo, ClientParameterCode.Object, false);
		}

		private void AddCharInfo(Fight fight)
		{
		    charsInfo = new Dictionary<int, CharFightInfo>();
			var allChars = fight.getAllParticipants();
			foreach(var player in allChars)
			{
				var info = new CharFightInfo() 
											{
												ObjectId = player.ObjectId,
												Name = player.Name,
												Team = fight.CharFightData[player].Team,
												stats = player.Stats.GetHealthLevel(),
												equipment = player.Items.Equipment.ToDictionary(k => (int)k.Key, v => (ItemData)(Item)v.Value)
											};
				charsInfo.Add(player.ObjectId, info);
				//cplayer.Client.Log.DebugFormat("FP {0} added to team {1} (client packet)", cplayer.Name, fight.CharFightData[cplayer.ObjectId].Team);
			}
		}

	}
}

