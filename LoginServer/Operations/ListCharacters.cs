using Photon.SocketServer;
using MMO.Framework;
using Photon.SocketServer.Rpc;
using ComplexServerCommon;

namespace LoginServer.Operations
{
	public class ListCharacters : Operation
	{
		public ListCharacters (IRpcProtocol protocol, IMessage message) : base (protocol, new OperationRequest (message.Code, message.Parameters))
		{
		}

		[DataMember(Code = (byte)ClientParameterCode.UserId, IsOptional = false)]
		public int UserId {get; set;}
	}
}

