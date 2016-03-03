using System;
using MMO.Photon.Server;
using MMO.Framework;
using ComplexServerCommon;
using MMO.Photon.Application;
using ComplexServerCommon.MessageObjects;
using System.Collections.Generic;
using Photon.SocketServer;
using SubServerCommon.Data.ClientData;


namespace ChatServer.Handlers
{
	public class ChatServerChatHandler : PhotonServerHandler
	{
		public ChatServerChatHandler(PhotonApplication application) : base (application)
		{
		}

		#region implemented abstract members of PhotonServerHandler

		public override MessageType Type
		{
			get
			{
				return MessageType.Request;
			}
		}
		
		public override byte Code
		{
			get
			{
				return (byte) ClientOperationCode.Chat;
			}
		}
		
		public override int? SubCode
		{
			get
			{
				return (int)MessageSubCode.Chat;
			}
		}

		protected override bool OnHandleMessage(IMessage message, PhotonServerPeer serverPeer)
		{
			if(message.Parameters.ContainsKey((byte)ClientParameterCode.Object))
			{
				Guid peerId = new Guid((Byte[])message.Parameters[(byte)ClientParameterCode.PeerId]);
				var chatItem = Xml.Deserialize<ChatItem>(message.Parameters[(byte)ClientParameterCode.Object]);

				switch(chatItem.Type)
				{
					case(ChatType.General):
						chatItem.ByPlayer = Server.ConnectionCollection<SubServerConnectionCollection>().Clients[peerId].ClientData<ChatPlayer>().CharacterName;
						//chatItem.Text - contains client's input string;
						foreach (var client in Server.ConnectionCollection<SubServerConnectionCollection>().Clients)
						{
							var para = new Dictionary<byte, object>()
										{
											{(byte)ClientParameterCode.PeerId, client.Key.ToByteArray()},
											{(byte)ClientParameterCode.Object, Xml.Serialize(chatItem)}
										};
							EventData eventData = new EventData{Code = (byte)ClientEventCode.Chat, Parameters = para};
							client.Value.ClientData<ServerData>().ServerPeer.SendEvent(eventData, new SendParameters());
						}
						break;
					default:
						break;
				}
			}
			return true;
		}



		#endregion
	}
}

