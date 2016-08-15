using System;
using System.Linq;
using ComplexServerCommon;
using ComplexServerCommon.MessageObjects;
using RegionServer.Model.Items;

namespace RegionServer.Model.ServerEvents.FightEvents
{
    public class UserFightInfoUpdatePacket : ServerPacket
	{
		public UserFightInfoUpdatePacket(CPlayerInstance instance) : base(ClientEventCode.ServerPacket, MessageSubCode.UserFightInfo)
		{
			AddParameter(instance.ObjectId, ClientParameterCode.ObjectId);
			AddUserInfo(instance);
		}

		void AddUserInfo(CPlayerInstance player)
		{
			UserFightInfo info = new UserFightInfo()
												{
													Name = player.Name,
													ObjectId = player.ObjectId,
													TargetId = player.Target.ObjectId,
													Team = player.CurrentFight.CharFightData[player].Team,
													stats = player.Stats.GetHealthLevel(), //add more later
													equipment = player.Items.Equipment.ToDictionary(k => k.Key, v => (ItemData)(Item)v.Value)
												};
			AddSerializedParameter(info, ClientParameterCode.Object, false);
		}
	}
}

