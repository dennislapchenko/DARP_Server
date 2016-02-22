using ComplexServerCommon;
using ComplexServerCommon.MessageObjects;

namespace RegionServer.Model.ServerEvents
{
	public class MoveToLocation : ServerPacket
	{
		public MoveToLocation(CCharacter character) : base(ClientEventCode.ServerPacket, MessageSubCode.MoveToLocation)
		{
			AddParameter(character.ObjectId, ClientParameterCode.ObjectId);
			AddSerializedParameter(new MoveTo {CurrentPosition = character.Position, Destination = character.Destination}, ClientParameterCode.Object);
		}
	}
}

