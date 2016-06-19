using ComplexServerCommon;
using System.Collections.Generic;
using ComplexServerCommon.MessageObjects;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using ComplexServerCommon.Enums;
using RegionServer.Model.Items;

namespace RegionServer.Model.ServerEvents
{
	public class FightParticipants : ServerPacket
	{
		List<KeyValuePairS<int, CharFightInfo>> charsInfo;

		private CPlayerInstance _instance;
		public FightParticipants(CPlayerInstance player, bool onlyTarget) : base(ClientEventCode.ServerPacket, MessageSubCode.FightParticipants)
		{
			_instance = player;
			AddParameter(player.TargetId, ClientParameterCode.ObjectId);
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
			charsInfo = new List<KeyValuePairS<int, CharFightInfo>>();
			var allChars = fight.Players;
			foreach(var player in allChars.Values)
			{
				var cplayer = player as CPlayerInstance;
				if(cplayer != null)
				{
					var info = new CharFightInfo() 
												{
													ObjectId = cplayer.ObjectId,
													Name = cplayer.Name,
													Team = (fight.CharFightData[cplayer].Team == FightTeam.Red ? FightTeam.Red : FightTeam.Blue),
													stats = cplayer.Stats.GetHealthLevel(),
													equipment = Util.ConvertEquipmentForXml(cplayer.Items.Equipment),
												};
					charsInfo.Add(new KeyValuePairS<int, CharFightInfo>(cplayer.ObjectId, info));
					//cplayer.Client.Log.DebugFormat("FP {0} added to team {1} (client packet)", cplayer.Name, fight.CharFightData[cplayer.ObjectId].Team);
				}
			}
		}

	}
}

