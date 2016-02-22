using MMO.Photon.Server;
using MMO.Photon.Application;
using MMO.Framework;
using ComplexServerCommon;
using System;
using MMO.Photon.Client;
using Photon.SocketServer;

namespace ComplexServer.Handlers
{
	public class RequestForwardHandler : DefaultRequestHandler
	{
		public RequestForwardHandler (PhotonApplication application) : base(application)
		{
		}

		#region implemented abstract members of PhotonServerHandler

		protected override bool OnHandleMessage (MMO.Framework.IMessage message, PhotonServerPeer serverPeer)
		{
			Log.ErrorFormat("No Existing Request Handler");
			return true;
		}

		public override MessageType Type {
			get {
				return MessageType.Request;
			}
		}

		public override byte Code {
			get {
				return (byte)(ClientOperationCode.Chat | ClientOperationCode.Login | ClientOperationCode.Region);
			}
		}

		public override int? SubCode {
			get {
				return null;
			}
		}

		#endregion
	}
}

