using System;
using MMO.Photon.Server;
using MMO.Photon.Application;
using MMO.Framework;
using ComplexServerCommon;
using ComplexServer;
using System.Collections.Generic;
using Photon.SocketServer;
using SubServerCommon;

namespace LoginServer.Handlers
{
	public class ComplexServerLogoutRequestHandler : PhotonServerHandler
	{
		PhotonApplication _application;
		public ComplexServerLogoutRequestHandler(PhotonApplication application) : base(application)
		{
			_application = application;
		}

		public override MessageType Type { get { return MessageType.Request;}}
		public override byte Code {get { return (byte)ClientOperationCode.Login;}}
		public override int? SubCode { get { return (int)MessageSubCode.Logout;}}

		protected override bool OnHandleMessage(IMessage message, PhotonServerPeer serverPeer)
		{
			var peerId = new Guid((Byte[])message.Parameters[(byte)ClientParameterCode.PeerId]);

			LoginServer login = Server as LoginServer;
			if (login != null)
			{
				login.ConnectionCollection<SubServerConnectionCollection>().Clients.Remove(peerId);
			}

			return true;
		}
	}
}

