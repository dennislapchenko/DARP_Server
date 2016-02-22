using System;
using ComplexServerCommon;
using ComplexServerCommon.MessageObjects;


namespace RegionServer.Model.ServerEvents
{
	public class CharInfoUpdate : ServerPacket
	{
		public CharInfoUpdate(CPlayerInstance player) : base(ClientEventCode.ServerPacket, MessageSubCode.CharInfo, false)
		{
			AddParameter(player.ObjectId, ClientParameterCode.ObjectId);
			AddCharInfo(player);
		}

		public void AddCharInfo(CPlayerInstance player)
		{
			CharInfo info = new CharInfo() 
			{
				Position = player.Position,
				Name = player.Name,

				//race, sex, class, title, guild
				//inventory - visible slot armor/weapon
				//effects - pvp flag, debuffs, buffs
				//movement speed for smoothing/calculation
				//action/emote walk, run, sit
			};
			AddSerializedParameter(info, ClientParameterCode.Object, false);
		}
	}
}

