using RegionServer.Model.Interfaces;
using ComplexServerCommon;

namespace RegionServer.Model.ServerEvents
{
	public class DeleteObjectPacket : ServerPacket
	{
		public DeleteObjectPacket(IObject obj) : base(ClientEventCode.ServerPacket, MessageSubCode.DeleteObject)
		{
			AddParameter(obj.ObjectId, ClientParameterCode.ObjectId);
		}

	}
}

