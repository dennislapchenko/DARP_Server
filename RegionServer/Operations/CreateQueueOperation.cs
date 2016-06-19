using System;
using Photon.SocketServer.Rpc;
using Photon.SocketServer;
using MMO.Framework;
using ComplexServerCommon;
using ComplexServerCommon.MessageObjects;

namespace RegionServer.Operations
{
	public class CreateQueueOperation : Operation
	{
		public CreateQueueOperation(IRpcProtocol protocol, IMessage message) 
			: base (protocol, new OperationRequest(message.Code, message.Parameters))
		{
		}

		[DataMember(Code = (byte)ClientParameterCode.Object, IsOptional = false)]
		public string fightInit { get; set;}
	}
}

