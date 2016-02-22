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

namespace ChatServer.Handlers
{
	public class ChatServerRegisterEventHandler : PhotonServerHandler
	{
		private readonly SubServerClientPeer.Factory _clientFactory;

		public ChatServerRegisterEventHandler(PhotonApplication application, SubServerClientPeer.Factory clientFactory) : base(application)
		{
			_clientFactory = clientFactory;
		}

		#region implemented abstract members of PhotonServerHandler

	

		public override MessageType Type
		{
			get
			{
				return MessageType.Async;
			}
		}

		public override byte Code
		{
			get
			{
				return (byte) ServerEventCode.CharacterRegister; 
			}
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
				using (var session = NHibernateHelper.OpenSession())
				{
					using (var transaction = session.BeginTransaction())
					{

						var character = session.QueryOver<ComplexCharacter>().Where(cc => cc.Id == characterId).List().FirstOrDefault();
						if (character != null)
						{
							transaction.Commit();
							var clients = Server.ConnectionCollection<SubServerConnectionCollection>().Clients;
							clients.Add(peerId, _clientFactory(peerId));
							//TODO: add character data to the client list for chat
							clients[peerId].ClientData<CharacterData>().CharacterId = characterId;
							clients[peerId].ClientData<CharacterData>().UserId = Convert.ToInt32(message.Parameters[(byte)ClientParameterCode.UserId]);
							clients[peerId].ClientData<ChatPlayer>().CharacterName = character.Name; 
							clients[peerId].ClientData<ServerData>().ServerPeer = serverPeer;
							Log.DebugFormat("Character {0} in chat server.", character.Name);
							//Notify guild members that someone logged in
							//Notify friend list that someone logged in
						}
						else
						{
							transaction.Commit();
							Log.FatalFormat("[ChatServerRegisterEventHandler] - Should not reach - Character not found in database");
						}

					}
				}
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

