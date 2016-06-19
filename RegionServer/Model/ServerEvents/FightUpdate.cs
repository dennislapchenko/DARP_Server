using System.Linq;
using ComplexServerCommon;
using ComplexServerCommon.MessageObjects;
using System.Collections.Generic;

namespace RegionServer.Model.ServerEvents
{
	public class FightUpdate : ServerPacket
	{
		List<KeyValuePairS<int, CharFightInfo>> charsInfo;

		public FightUpdate(CPlayerInstance instance, Dictionary<int, ExchangeProfile> result) : base(ClientEventCode.ServerPacket, MessageSubCode.FightUpdate)
		{
			AddHealthLevelOnly(instance, result);
		}

		private void AddHealthLevelOnly(CPlayerInstance instance, Dictionary<int, ExchangeProfile> result)
		{
			var fight = instance.CurrentFight;
			charsInfo = new List<KeyValuePairS<int, CharFightInfo>>();
			var allChars = fight.Players;
			foreach(var cplayer in allChars.Values)
			{
				if(cplayer != null)
				{
					var info = new CharFightInfo() 
												{
													Team = cplayer.CurrentFight.CharFightData[cplayer].Team,
													stats = cplayer.Stats.GetHealthLevel(),
												};
					charsInfo.Add(new KeyValuePairS<int, CharFightInfo>(cplayer.ObjectId, info));
				}
			}

			var serializedExchangeList = new List<KeyValuePairS<int, ExchangeProfile>>();
			foreach(var r in result)
			{
				serializedExchangeList.Add(new KeyValuePairS<int, ExchangeProfile>(r.Key, r.Value));
			}

			var fightInfo = new FightInitInfo()
			{
				allChars = charsInfo,
				moveLog = serializedExchangeList,
			};
			AddSerializedParameter(fightInfo, ClientParameterCode.Object, false);
		}
	}
}

