using Photon.SocketServer.Rpc;
using Photon.SocketServer;
using MMO.Framework;
using ComplexServerCommon;

namespace LoginServer.Operations
{
	class CreateCharacter : Operation
	{
		public CreateCharacter(IRpcProtocol protocol, IMessage message) : base(protocol, new OperationRequest(message.Code, message.Parameters))
		{
		}

		[DataMember(Code = (byte)ClientParameterCode.UserId, IsOptional = false)]
		public int UserId {get; set;}
		[DataMember(Code = (byte)ClientParameterCode.CharacterCreateDetails, IsOptional = false)]
		public string CharacterCreateDetails {get; set;}
	}

}

