using RegionServer.Model.Interfaces;
using ComplexServerCommon;

namespace RegionServer.Model.ServerEvents
{
	public class DeleteObject : ServerPacket
	{
		public DeleteObject(IObject obj) : base(ClientEventCode.ServerPacket, MessageSubCode.DeleteObject)
		{
			AddParameter(obj.ObjectId, ClientParameterCode.ObjectId);
		}

	}
}

