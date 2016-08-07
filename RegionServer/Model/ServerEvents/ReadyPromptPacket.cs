using System;
using ComplexServerCommon;

namespace RegionServer.Model.ServerEvents
{
	public class ReadyPromptPacket : ServerPacket
	{
		public ReadyPromptPacket() : base(ClientEventCode.ServerPacket, MessageSubCode.ReadyPrompt)
		{
		}
	}
}

