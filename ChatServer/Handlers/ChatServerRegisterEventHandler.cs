using MMO.Photon.Server;
using MMO.Photon.Application;
using MMO.Photon.Client;
using MMO.Framework;
using SubServerCommon;
using System;
using System.Linq;
using ComplexServerCommon;
using NHibernate.Exceptions;
using SubServerCommon.Data.NHibernate;
using SubServerCommon.Data.ClientData;

namespace ChatServer.Handlers
{
	public class ChatServerRegisterEventHandler : PhotonServerHandler
	{
		private readonly SubServerClientPeer.Factory _clientFactory;
	    private readonly string CLASSNAME = "ChatServerRegisterEventHandler";

		public ChatServerRegisterEventHandler(PhotonApplication application, SubServerClientPeer.Factory clientFactory) : base(application)
		{
			_clientFactory = clientFactory;
		}

		#region implemented abstract members of PhotonServerHandler

		public override MessageType Type
		{
			get {   return MessageType.Async;   }
		}

		public override byte Code
		{
			get {   return (byte) ServerEventCode.CharacterRegister;    }
		}

		public override int? SubCode
		{
			get	{   return null;    }
		}

        /** Handles character login event from Proxy server.
         * Receives peerId and characterId in the message, queries character info in DB.
         * Then instantiates and adds character object to Clients dictionary and populates character info fields.
         * Fields are later used by chat message handlers to have additional information.
         * 
         * TODO: Notifies friends/guild members that character is online
         */

		protected override bool OnHandleMessage(IMessage message, PhotonServerPeer serverPeer)
		{
			var characterId = Convert.ToInt32(message.Parameters[(byte)ClientParameterCode.CharacterId]);
			var peerId = new Guid((Byte[])message.Parameters[(byte)ClientParameterCode.PeerId]);

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
						    throw new SqlParseException(CLASSNAME + " Character not found in database");
						}

					}
				}
			}
			catch(Exception e)
			{
			    Log.Error(e.StackTrace);
				throw;
			}
			return true;
		}

		#endregion
	}
}

