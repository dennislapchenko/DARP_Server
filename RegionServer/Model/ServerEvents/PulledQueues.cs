using System;
using ComplexServerCommon;
using RegionServer.Model.Fighting;

namespace RegionServer.Model.ServerEvents
{
	public class PulledQueues : ServerPacket
	{
		public PulledQueues(FightManager fightManager) : base(ClientEventCode.ServerPacket, MessageSubCode.PullQueue)
		{
			AddSerializedParameter(fightManager.GetAllQueues(), ClientParameterCode.Object);
		}
	}
}

