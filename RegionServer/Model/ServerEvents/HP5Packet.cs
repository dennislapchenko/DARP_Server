using System;
using ComplexServerCommon;
using RegionServer.Model.Stats;

namespace RegionServer.Model.ServerEvents
{
	public class HP5Packet : ServerPacket
	{
        private int newHealth;

        public HP5Packet(CPlayerInstance instance, int newHealth) : base (ClientEventCode.ServerPacket, MessageSubCode.RegenUpdates)
		{
			AddParameter(newHealth, ClientParameterCode.CurrentHealth);
		}
    }
}

