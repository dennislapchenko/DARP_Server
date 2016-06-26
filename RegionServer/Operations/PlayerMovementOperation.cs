using Photon.SocketServer.Rpc;
using Photon.SocketServer;
using MMO.Framework;
using ComplexServerCommon;


namespace RegionServer.Operations
{
	public class PlayerMovementOperation : Operation
	{
		public PlayerMovementOperation(IRpcProtocol protocol, IMessage message) 
			: base (protocol, new OperationRequest(message.Code, message.Parameters))
		{
		}

		[DataMember(Code = (byte)ClientParameterCode.UserId, IsOptional = false)]
		public int UserId { get; set;}

		[DataMember(Code = (byte)ClientParameterCode.Object, IsOptional = false)]
		public string PlayerMovement {get; set;}
	}
}

