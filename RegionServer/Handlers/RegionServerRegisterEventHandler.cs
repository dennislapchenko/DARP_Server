using MMO.Photon.Server;
using MMO.Photon.Application;
using MMO.Photon.Client;
using MMO.Framework;
using SubServerCommon;
using System;
using System.Linq;
using ComplexServerCommon;
using SubServerCommon.Data.NHibernate;
using SubServerCommon.Data.ClientData;
using RegionServer.Model;

namespace RegionServer.Handlers
{
	public class RegionServerRegisterEventHandler : PhotonServerHandler
	{
		private readonly SubServerClientPeer.Factory _clientFactory;

		public RegionServerRegisterEventHandler(PhotonApplication application, SubServerClientPeer.Factory clientFactory) : base(application)
		{
			_clientFactory = clientFactory;
		}

		#region implemented abstract members of PhotonServerHandler

	

		public override MessageType Type
		{
			get	{ return MessageType.Async;	}
		}

		public override byte Code
		{
			get	{ return (byte) ServerEventCode.CharacterRegister; }
		}

		public override int? SubCode
		{
			get	{ return null; }
		}

		protected override bool OnHandleMessage(IMessage message, PhotonServerPeer serverPeer)
		{
			int characterId = Convert.ToInt32(message.Parameters[(byte)ClientParameterCode.CharacterId]);
			Guid peerId = new Guid((Byte[])message.Parameters[(byte)ClientParameterCode.PeerId]);
			try
			{
				var clients = Server.ConnectionCollection<SubServerConnectionCollection>().Clients;
				clients.Add(peerId, _clientFactory(peerId));
				var instance = clients[peerId].ClientData<CPlayerInstance>();

				instance.UserID = Convert.ToInt32(message.Parameters[(byte)ClientParameterCode.UserId]);
				instance.ServerPeer = serverPeer;
				instance.Client = clients[peerId];
				instance.CharacterID = characterId;
				instance.ObjectId = characterId;

				instance.Restore();
			}
			catch(Exception e)
			{
				Log.Error(e);
				throw;
			}
			return true;
		}

		#endregion
	}
}

