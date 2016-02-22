using MMO.Photon.Server;
using MMO.Photon.Application;
using MMO.Framework;
using ComplexServerCommon;
using System;
using MMO.Photon.Client;
using Photon.SocketServer;

namespace ComplexServer.Handlers
{
	public class EventForwardHandler : DefaultEventHandler
	{
		public EventForwardHandler (PhotonApplication application) : base(application)
		{
		}
		protected override bool OnHandleMessage (MMO.Framework.IMessage message, PhotonServerPeer serverPeer)
		{
			if (message.Parameters.ContainsKey((byte) ClientParameterCode.PeerId))
			{
				PhotonClientPeer peer;
				Server.ConnectionCollection<PhotonConnectionCollection>().Clients.TryGetValue(new Guid((Byte[])message.Parameters[(byte)ClientParameterCode.PeerId]), out peer);
				if (peer != null)
				{
					message.Parameters.Remove((byte) ClientParameterCode.PeerId);
					peer.SendEvent(new EventData(message.Code, message.Parameters), new SendParameters());
				}
			}
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
	}
}

