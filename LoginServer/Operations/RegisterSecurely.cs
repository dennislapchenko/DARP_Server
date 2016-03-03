using Photon.SocketServer.Rpc;
using Photon.SocketServer;
using MMO.Framework;
using ComplexServerCommon;


namespace LoginServer.Operations
{
	public class RegisterSecurely : Operation
	{
		public RegisterSecurely (IRpcProtocol rpcProtocol, IMessage message) : base(rpcProtocol, new OperationRequest(message.Code, message.Parameters))
		{
		}

		[DataMember(Code = (byte)ClientParameterCode.UserName, IsOptional = false)]
		public string UserName {get; set;}
		[DataMember(Code = (byte)ClientParameterCode.Password, IsOptional = false)]
		public string Password {get; set;}
		[DataMember(Code = (byte)ClientParameterCode.Email, IsOptional = false)]
		public string Email {get; set;}
	}

}

