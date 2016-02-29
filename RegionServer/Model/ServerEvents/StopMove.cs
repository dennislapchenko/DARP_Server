using ComplexServerCommon;
using ComplexServerCommon.MessageObjects;


namespace RegionServer.Model.ServerEvents
{
	public class StopMove : ServerPacket
	{
		public StopMove(CCharacter character) : base(ClientEventCode.ServerPacket, MessageSubCode.StopMove)
		{
			AddParameter(character.ObjectId, ClientParameterCode.ObjectId);
			AddSerializedParameter((PositionData)character.Destination, ClientParameterCode.Object);
		}

		public StopMove(int objectId, float x, float y, float z, short heading) : base (ClientEventCode.ServerPacket, MessageSubCode.StopMove)
		{
			AddParameter(objectId, ClientParameterCode.ObjectId);
			AddSerializedParameter(new PositionData(x, y, z, heading), ClientParameterCode.Object);
		}
	}
}

