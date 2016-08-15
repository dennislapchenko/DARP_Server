using System;
using ComplexServerCommon;
using ComplexServerCommon.MessageObjects;

namespace RegionServer.Model.ServerEvents.FightEvents
{
    public class FinishFightPacket : ServerPacket
	{
		public FinishFightPacket(Rewards reward) : base(ClientEventCode.ServerPacket, MessageSubCode.FinishFight)
		{
			AddSerializedParameter(reward, ClientParameterCode.Object);
		}
	}
}

