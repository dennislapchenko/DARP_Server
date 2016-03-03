using Photon.SocketServer.Rpc;
using Photon.SocketServer;
using MMO.Framework;


namespace SubServerCommon.Operations
{
	public class RegisterSubServer : Operation
	{
		public RegisterSubServer (IRpcProtocol rpcProtocol, IMessage message)
			: base(rpcProtocol, new OperationRequest(message.Code, message.Parameters))
		{

		}

		public RegisterSubServer()
		{

		}

		public RegisterSubServer (IRpcProtocol rpcProtocol, OperationRequest operationRequest)
			: base(rpcProtocol, operationRequest)
		{
			
		}

		[DataMember(Code = (byte)ServerParameterCode.RegisterSubServerOperation, IsOptional = false)] 
		public string RegisterSubServerOperation {get; set;}
	}
}

