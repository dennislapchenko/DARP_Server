using ComplexServerCommon;
using ComplexServerCommon.Enums;


namespace RegionServer.Model.ServerEvents
{
	public class TeleportToLocationPacket : ServerPacket
	{
		public TeleportToLocationPacket(CObject obj, LocationType high, LocationType low) : this(obj, new Position(high, low))
		{
		}

		public TeleportToLocationPacket(CObject obj, Position pos) : base(ClientEventCode.ServerPacket, MessageSubCode.TeleportToLocation)
		{
			AddParameter(obj.ObjectId, ClientParameterCode.ObjectId);
			AddSerializedParameter(pos, ClientParameterCode.Object);
		}
	}
}

