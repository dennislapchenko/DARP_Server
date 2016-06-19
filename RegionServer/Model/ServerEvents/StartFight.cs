using System;
using ComplexServerCommon;

namespace RegionServer.Model.ServerEvents
{
	public class StartFight : ServerPacket
	{
		public StartFight() : base(ClientEventCode.ServerPacket, MessageSubCode.StartFight)
		{
			
		}
	}
}

