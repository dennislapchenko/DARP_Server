using MMO.Photon.Client;
using MMO.Photon.Application;
using MMO.Framework;
using ComplexServerCommon;
using SubServerCommon.Data.ClientData;
using Photon.SocketServer;
using SubServerCommon;


namespace ComplexServer.Handlers
{
	public class HandleClientChatRequests : PhotonClientHandler
	{
		public HandleClientChatRequests (PhotonApplication application) : base(application)
		{
		}
		
		
		public override MessageType Type {
			get {
				return MessageType.Async | MessageType.Request | MessageType.Response;
			}
		}
		public override byte Code {
			get {
				return (byte) ClientOperationCode.Chat;
			}
		}
		public override int? SubCode {
			get {
				return null;
			}
		}
		protected override bool OnHandleMessage (IMessage message, PhotonClientPeer peer)
		{
			Log.DebugFormat("Handling Client Chat Request");

			#region Purge & Replace (PeerId, UserId, CharacterId)
			message.Parameters.Remove((byte)ClientParameterCode.PeerId);
			message.Parameters.Add((byte) ClientParameterCode.PeerId, peer.PeerId.ToByteArray());
			message.Parameters.Remove((byte) ClientParameterCode.UserId);
			if (peer.ClientData<CharacterData>().UserId.HasValue)
			{
				message.Parameters.Add( (byte)ClientParameterCode.UserId, peer.ClientData<CharacterData>().UserId);
			}
			if(peer.ClientData<CharacterData>().CharacterId.HasValue)
			{
				message.Parameters.Remove((byte)ClientParameterCode.CharacterId);
				message.Parameters.Add((byte)ClientParameterCode.CharacterId, peer.ClientData<CharacterData>().CharacterId);
			}
			#endregion

			var operationRequest = new OperationRequest(message.Code, message.Parameters);
			switch (message.Code)
			{
				case (byte)ClientOperationCode.Chat:
					if (Server.ConnectionCollection<PhotonConnectionCollection>() != null)
					{
						Server.ConnectionCollection<PhotonConnectionCollection>().GetServerByType((int)ServerType.Chat).SendOperationRequest(operationRequest, new SendParameters());
					}
					break;
				default:
					Log.DebugFormat("Invalid Operation Code - Expecting Chat");
					break;
			}
			return true;
		}
	}
}
