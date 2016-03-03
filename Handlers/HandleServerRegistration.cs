using MMO.Photon.Server;
using MMO.Framework;
using MMO.Photon.Application;
using Photon.SocketServer;
using SubServerCommon.Operations;
using ComplexServerCommon;
using SubServerCommon.Data;
using SubServerCommon;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace ComplexServer.Handlers
{
	public class HandleServerRegistration : PhotonServerHandler
	{
		public HandleServerRegistration (PhotonApplication application) : base(application)
		{

		}
		#region implemented abstract members of PhotonServerHandler

		protected override bool OnHandleMessage (IMessage message, PhotonServerPeer serverPeer)
		{
			OperationResponse operationResponse;
			if (serverPeer.ServerId.HasValue)
			{
				operationResponse = new OperationResponse(message.Code) { ReturnCode = -1, DebugMessage = "Already Registered"};
			}
			else
			{
				var registerRequest = new RegisterSubServer(serverPeer.Protocol, message);
				if (!registerRequest.IsValid)
				{
					string msg = registerRequest.GetErrorMessage();
					if (Log.IsDebugEnabled)
					{
						Log.DebugFormat("Invalid Register Request: {0}", msg);
					}

					operationResponse = new OperationResponse(message.Code) { DebugMessage = msg, ReturnCode = (short)ErrorCode.OperationInvalid};
				}
				else
				{
					var registerData = Xml.Deserialize<RegisterSubServerData>(registerRequest.RegisterSubServerOperation);
					if (Log.IsDebugEnabled)
					{
						Log.DebugFormat("Received register request: Address={0}, UdpPort={2}, TcpPort={1}, Type={3}",
						    registerData.GameServerAddress, registerData.TcpPort, registerData.UdpPort, registerData.ServerType);
					}
					if (registerData.UdpPort.HasValue)
					{
						serverPeer.UdpAddress = registerData.GameServerAddress + ":" + registerData.UdpPort;
					}
					if (registerData.TcpPort.HasValue)
					{
						serverPeer.TcpAddress = registerData.GameServerAddress + ":" + registerData.TcpPort;
					}
					serverPeer.ServerId = registerData.ServerId;
					serverPeer.ServerType = registerData.ServerType;

					serverPeer.ApplicationName = registerData.ApplicationName;
					Server.ConnectionCollection<PhotonConnectionCollection>().OnConnect(serverPeer);
					operationResponse = new OperationResponse(message.Code);
				}
			}
			serverPeer.SendOperationResponse(operationResponse, new SendParameters());
//			string FilePath = Path.Combine(Server.BinaryPath, "esquel.dar");
//			List<string> sqlsetup = File.ReadLines(FilePath).ToList();
//			Log.DebugFormat("{0}\n{1}\n{2}\n{3}", sqlsetup[0],sqlsetup[1],sqlsetup[1],sqlsetup[1]);
			return true;
		}

		public override MessageType Type {
			get {
				return MessageType.Request;
			}
		}

		public override byte Code {
			get {
				return (byte)ServerOperationCode.RegisterSubServer;
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

