using System;
using ComplexServerCommon;

namespace RegionServer.Model.ServerEvents.FightEvents
{
    public class StartFightPacket : ServerPacket
	{
		public StartFightPacket() : base(ClientEventCode.ServerPacket, MessageSubCode.StartFight)
		{
			
		}
	}
}

