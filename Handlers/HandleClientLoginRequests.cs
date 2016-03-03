using MMO.Photon.Client;
using MMO.Photon.Application;
using ComplexServerCommon;
using Photon.SocketServer;
using SubServerCommon;
using SubServerCommon.Data.ClientData;
using MMO.Framework;


namespace ComplexServer.Handlers
{
	public class HandleClientLoginRequests : PhotonClientHandler
	{
		public HandleClientLoginRequests (PhotonApplication application) : base(application)
		{
		}

		#region implemented abstract members of PhotonClientHandler

		public override MessageType Type {
			get {
				return MessageType.Async | MessageType.Request | MessageType.Response;
			}
		}
		public override byte Code {
			get {
				return (byte) ClientOperationCode.Login;
			}
		}
		public override int? SubCode {
			get {
				return null;
			}
		}
		protected override bool OnHandleMessage (IMessage message, PhotonClientPeer peer)
		{
			Log.DebugFormat("Handling Client Login Request");

			#region Security peerid, userid, charid purge & replace
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
				case (byte)ClientOperationCode.Login:
					if (Server.ConnectionCollection<PhotonConnectionCollection>() != null)
					{
						Server.ConnectionCollection<PhotonConnectionCollection>().GetServerByType((int)ServerType.Login).SendOperationRequest(operationRequest, new SendParameters());
					}
					break;
			default:
				Log.DebugFormat("Invalid Operation Code - Expecting Login");
				break;
			}
			return true;
		}
		#endregion
	}
}
