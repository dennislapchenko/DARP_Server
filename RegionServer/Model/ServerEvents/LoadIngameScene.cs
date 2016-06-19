using System;
using ComplexServerCommon;

namespace RegionServer.Model.ServerEvents
{
	public class LoadIngameScene : ServerPacket
	{
		public LoadIngameScene() : base(ClientEventCode.ServerPacket, MessageSubCode.LoadIngameScene)
		{
		}
	}
}

