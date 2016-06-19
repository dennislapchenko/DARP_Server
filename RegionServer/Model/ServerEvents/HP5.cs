using System;
using ComplexServerCommon;
using RegionServer.Model.Stats;

namespace RegionServer.Model.ServerEvents
{
	public class HP5 : ServerPacket
	{
		public HP5(CPlayerInstance instance) : base (ClientEventCode.ServerPacket, MessageSubCode.RegenUpdates)
		{
			AddParameter((int)instance.Stats.GetStat<CurrHealth>(), ClientParameterCode.CurrentHealth);
		}
	}
}

