using System;
using MMO.Photon.Server;
using MMO.Photon.Application;
using MMO.Framework;
using ComplexServerCommon;
using RegionServer.Model;
using System.Collections.Generic;
using Photon.SocketServer;
using RegionServer.Model.ServerEvents;


namespace RegionServer.Handlers
{
	public class PlayerInFightHandler : PhotonServerHandler
	{
		public PlayerInFightHandler(PhotonApplication application) : base(application)
		{		
		}

		public override MessageType Type { get { return MessageType.Request; } }
		public override byte Code { get { return (byte)ClientOperationCode.Region; } }
		public override int? SubCode { get { return (int)MessageSubCode.PlayerInFight; } }

		protected override bool OnHandleMessage(IMessage message, PhotonServerPeer serverPeer)
		{
			var instance = Util.GetCPlayerInstance(Server, message);

			//send battleinfo to player for UI update
			instance.SendPacket(new UserFightInfoUpdatePacket(instance));
			instance.SendPacket(new FightParticipantsPacket(instance, false));
			return true;
		}

	}
}