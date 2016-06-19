using System;
using Photon.SocketServer.Rpc;
using Photon.SocketServer;
using MMO.Framework;
using ComplexServerCommon;

namespace RegionServer.Operations
{
	public class ItemEquipOperation : Operation
	{
		public ItemEquipOperation(IRpcProtocol protocol, IMessage message) 
			: base (protocol, new OperationRequest(message.Code, message.Parameters))
		{
		}

		[DataMember(Code = (byte)ClientParameterCode.UserId, IsOptional = false)]
		public int UserId { get; set;}
		[DataMember(Code = (byte)ClientParameterCode.InventorySlot, IsOptional = false)]
		public int InventorySlot {get; set;}
	}
}

