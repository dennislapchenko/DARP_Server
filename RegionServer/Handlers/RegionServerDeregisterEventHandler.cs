using MMO.Photon.Server;
using MMO.Framework;
using SubServerCommon;
using MMO.Photon.Application;
using System;
using ComplexServerCommon;
using RegionServer.Model;


namespace RegionServer.Handlers
{
	public class RegionServerDeregisterEventHandler : PhotonServerHandler
	{
		public RegionServerDeregisterEventHandler(PhotonApplication application) : base(application)
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

		protected override bool OnHandleMessage(IMessage message, PhotonServerPeer serverPeer)
		{
			Guid peerId = new Guid((Byte[])message.Parameters[(byte)ClientParameterCode.PeerId]);
			// remove from Groups, Guilds, etc.
			var instance = Server.ConnectionCollection<SubServerConnectionCollection>().Clients[peerId].ClientData<CPlayerInstance>();
			instance.Decay();

			Server.ConnectionCollection<SubServerConnectionCollection>().Clients.Remove(peerId);
			Log.DebugFormat("Removed Peer {0} from Region and Decayed instance, now we have {1} clients.", peerId, Server.ConnectionCollection<SubServerConnectionCollection>().Clients.Count);
			return true;
		}
	}
}

