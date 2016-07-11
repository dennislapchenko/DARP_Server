﻿using System;
using System.Linq;
using ComplexServerCommon;
using ComplexServerCommon.MessageObjects;

namespace RegionServer.Model.ServerEvents
{
	public class UserFightInfoUpdate : ServerPacket
	{
		public UserFightInfoUpdate(CPlayerInstance instance) : base(ClientEventCode.ServerPacket, MessageSubCode.UserFightInfo)
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
													equipment = player.Items.Equipment.ToDictionary(k => k.Key, v => (ItemData)v.Value)
												};
			AddSerializedParameter(info, ClientParameterCode.Object, false);
		}
	}
}

