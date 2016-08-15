using ComplexServerCommon;
using ComplexServerCommon.MessageObjects;
using System.Collections.Generic;

namespace RegionServer.Model.ServerEvents.FightEvents
{
    public class FightUpdatePacket : ServerPacket
	{
		Dictionary<int, CharFightInfo> charsInfo;

		public FightUpdatePacket(CPlayerInstance instance, Dictionary<int, ExchangeProfile> result) : base(ClientEventCode.ServerPacket, MessageSubCode.FightUpdate)
		{
			AddHealthLevelOnly(instance, result);
		}

		private void AddHealthLevelOnly(CPlayerInstance instance, Dictionary<int, ExchangeProfile> result)
		{
			var fight = instance.CurrentFight;
			charsInfo = new Dictionary<int, CharFightInfo>();
			var allChars = fight.getAllParticipants();
			foreach(var player in allChars)
			{
				var info = new CharFightInfo() 
											{
												Team = player.CurrentFight.CharFightData[player].Team,
												stats = player.Stats.GetHealthLevel(),
											};
				charsInfo.Add(player.ObjectId, info);
			}

			var fightInfo = new FightInitInfo()
			{
				allChars = charsInfo,
				moveLog = result
			};
			AddSerializedParameter(fightInfo, ClientParameterCode.Object, false);
		}
	}
}

