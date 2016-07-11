using MMO.Photon.Server;
using MMO.Photon.Application;
using MMO.Framework;
using ComplexServerCommon;
using System.Collections.Generic;
using Photon.SocketServer;
using ComplexServerCommon.MessageObjects;
using RegionServer.Operations;


namespace RegionServer.Handlers
{
	public class PlayerMovementHandler : PhotonServerHandler
	{
		public PlayerMovementHandler(PhotonApplication application) : base(application)
		{
		}

	    public override MessageType Type { get; }
	    public override byte Code { get; }
	    public override int? SubCode { get; }

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
            var instance = Util.GetCPlayerInstance(Server, message);
			var playerMovement = ComplexServerCommon.SerializeUtil.Deserialize<PlayerMovement>(operation.PlayerMovement);
            
            //implement movement logic

			return true;
		}
	}
}

