using System;
using ComplexServerCommon;
using ComplexServerCommon.MessageObjects;

namespace RegionServer.Model.ServerEvents
{
	public class FinishFight : ServerPacket
	{
		public FinishFight(Rewards reward) : base(ClientEventCode.ServerPacket, MessageSubCode.FinishFight)
		{
			AddSerializedParameter(reward, ClientParameterCode.Object);
		}
	}
}

