using System;
using ComplexServerCommon;

namespace RegionServer.Model.ServerEvents
{
	public class LoadIngameScenePacket : ServerPacket
	{
		public LoadIngameScenePacket() : base(ClientEventCode.ServerPacket, MessageSubCode.LoadIngameScene)
		{
		}
	}
}

