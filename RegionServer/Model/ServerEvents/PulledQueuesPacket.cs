using System;
using ComplexServerCommon;
using RegionServer.Model.Fighting;

namespace RegionServer.Model.ServerEvents
{
	public class PulledQueuesPacket : ServerPacket
	{
		public PulledQueuesPacket(FightManager fightManager) : base(ClientEventCode.ServerPacket, MessageSubCode.PullQueue)
		{
			AddSerializedParameter(fightManager.GetAllQueues(), ClientParameterCode.Object);
		}
	}
}

