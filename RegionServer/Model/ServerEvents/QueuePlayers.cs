using System;
using ComplexServerCommon;

namespace RegionServer.Model.ServerEvents
{
	public class QueuePlayers : ServerPacket
	{
		public QueuePlayers(CPlayerInstance player, Fight fight) : base(ClientEventCode.ServerPacket, MessageSubCode.FightParticipants)
		{
			AddParameter(player.ObjectId, ClientParameterCode.ObjectId);
			AddPlayersInfo(fight);
		}

		private void AddPlayersInfo(Fight fight)
		{

		}
	}
}

