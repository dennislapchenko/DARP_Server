using System;
using ComplexServerCommon;

namespace RegionServer.Model.ServerEvents
{
	public class ReadyPrompt : ServerPacket
	{
		public ReadyPrompt() : base(ClientEventCode.ServerPacket, MessageSubCode.ReadyPrompt)
		{
		}
	}
}

