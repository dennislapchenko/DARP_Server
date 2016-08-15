using MMO.Photon.Application;
using ComplexServerCommon;
using RegionServer.Model;
using System.Collections.Generic;
using Photon.SocketServer;
using MMO.Photon.Server;
using MMO.Framework;
using ComplexServerCommon.Enums;
using RegionServer.Model.Fighting;
using RegionServer.Model.ServerEvents;
using RegionServer.Model.ServerEvents.FightEvents;

namespace RegionServer.Handlers
{
	public class LeaveQueueHandler : PhotonServerHandler
	{
		private FightManager _fightManager;

		public LeaveQueueHandler(PhotonApplication application, FightManager fightManager) : base(application)
		{
			_fightManager = fightManager;
		}

		public override MessageType Type { get { return MessageType.Request; } }
		public override byte Code { get { return (byte)ClientOperationCode.Region; } }
		public override int? SubCode { get { return (int) MessageSubCode.LeaveQueue; } }

		protected override bool OnHandleMessage(IMessage message, PhotonServerPeer serverPeer)
		{
			var para = new Dictionary<byte, object>()
			{
				{(byte)ClientParameterCode.PeerId, message.Parameters[(byte)ClientParameterCode.PeerId]},
				{(byte)ClientParameterCode.SubOperationCode, MessageSubCode.PullQueue},
			};

			var instance = Util.GetCPlayerInstance(Server, message);
			var fight = instance.CurrentFight;
			var success = _fightManager.LeaveQueue(instance);

			if(success)
			{
				instance.SendPacket(new PulledQueuesPacket(_fightManager));
				if(fight != null)
				{
					var fightParticipants = new FightQueueParticipantsPacket(fight);
					foreach(var player in fight.getPlayers.Values)
					{
						player.SendPacket(new PulledQueuesPacket(_fightManager));
						player.SendPacket(fightParticipants);
					}
				}
			}
			else
			{
				serverPeer.SendOperationResponse(new OperationResponse(message.Code) { ReturnCode = (int)ErrorCode.OperationInvalid, DebugMessage = "Cannot leave this queue", Parameters = para}, new SendParameters());
			}
			return true;
		}


	}
}