using System;
using MMO.Photon.Server;
using MMO.Photon.Application;
using MMO.Framework;
using ComplexServerCommon;
using System.Collections.Generic;
using Photon.SocketServer;
using RegionServer.Model;
using ComplexServerCommon.MessageObjects;
using RegionServer.Operations;


namespace RegionServer.Handlers
{
	public class PlayerMovementHandler : PhotonServerHandler
	{
		public PlayerMovementHandler(PhotonApplication application) : base(application)
		{
		}


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
				return (byte)ClientOperationCode.Region;
			}
		}

		public override int? SubCode
		{
			get
			{
				return (int)MessageSubCode.PlayerMovement;
			}
		}

		protected override bool OnHandleMessage(IMessage message, PhotonServerPeer serverPeer)
		{
			var para = new Dictionary<byte, object>
			{
				{(byte)ClientParameterCode.PeerId, message.Parameters[(byte)ClientParameterCode.PeerId]},
				{(byte)ClientParameterCode.SubOperationCode, message.Parameters[(byte)ClientParameterCode.SubOperationCode]}
			};

			var operation = new PlayerMovementOperation(serverPeer.Protocol, message);

			//IF INCORRECT
			if (!operation.IsValid)
			{
				Log.ErrorFormat(operation.GetErrorMessage());
				serverPeer.SendOperationResponse(new OperationResponse(message.Code)
				{
					ReturnCode = (int)ErrorCode.OperationInvalid,
					DebugMessage = operation.GetErrorMessage(),
					Parameters = para
				}, new SendParameters());
				return true;
			}
			//WHEN CORRECT
			Guid peerId = new Guid((Byte[])message.Parameters[(byte)ClientParameterCode.PeerId]);
			var clients = Server.ConnectionCollection<SubServerConnectionCollection>().Clients;
			var instance = clients[peerId].ClientData<CPlayerInstance>();

			var playerMovement = Xml.Deserialize<PlayerMovement>(operation.PlayerMovement);
			instance.Physics.Movement = playerMovement;
			instance.Facing = playerMovement.Facing;

			return true;
		}
	}
}

