using System;
using Photon.SocketServer;
using MMO.Framework;
using ComplexServerCommon;
using Photon.SocketServer.Rpc;
using RegionServer.Model.Items;

namespace RegionServer.Operations
{
	public class ItemDequipOperation : Operation
	{
		public ItemDequipOperation(IRpcProtocol protocol, IMessage message) 
			: base (protocol, new OperationRequest(message.Code, message.Parameters))
		{
		}

		[DataMember(Code = (byte)ClientParameterCode.UserId, IsOptional = false)]
		public int UserId { get; set;}
		[DataMember(Code = (byte)ClientParameterCode.EquipmentSlot, IsOptional = false)]
		public int EquipmentSlot {get; set;}
	}
}

