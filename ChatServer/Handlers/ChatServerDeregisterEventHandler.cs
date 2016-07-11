using MMO.Photon.Server;
using MMO.Framework;
using SubServerCommon;
using MMO.Photon.Application;
using System;
using ComplexServerCommon;


namespace ChatServer.Handlers
{
	public class ChatServerDeregisterEventHandler : PhotonServerHandler
	{
		public ChatServerDeregisterEventHandler(PhotonApplication application) : base(application)
		{
		}

		public override MessageType Type
		{
			get { return MessageType.Async; }
		}

		public override byte Code
		{
			get { return (byte) ServerEventCode.CharacterDeregister; }
		}

		public override int? SubCode
		{
			get { return null; }
		}

        /** Receives players id to disconnect from chat server, builds Guid from that and removes this character from Clients dictionary
         * 
         *  Possibly other chat server character logout clean-up tasks here
         */

		protected override bool OnHandleMessage(IMessage message, PhotonServerPeer serverPeer)
		{
			Guid peerId = new Guid((Byte[])message.Parameters[(byte)ClientParameterCode.PeerId]);
			// remove from Groups, Guilds, etc.

			Server.ConnectionCollection<SubServerConnectionCollection>().Clients.Remove(peerId);
			Log.DebugFormat("Removed Peer {0} from Chat, now we have {1} clients.", peerId, Server.ConnectionCollection<SubServerConnectionCollection>().Clients.Count);
			return true;
		}
	}
}

