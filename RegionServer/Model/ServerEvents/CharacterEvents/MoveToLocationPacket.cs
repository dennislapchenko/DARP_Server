using ComplexServerCommon;
using ComplexServerCommon.MessageObjects;

namespace RegionServer.Model.ServerEvents.CharacterEvents
{
	public class MoveToLocationPacket : ServerPacket
	{
		public MoveToLocationPacket(CCharacter character) : base(ClientEventCode.ServerPacket, MessageSubCode.MoveToLocation)
		{
			AddParameter(character.ObjectId, ClientParameterCode.ObjectId);
			AddSerializedParameter(new MoveTo 
			                     	{
										CurrentPosition = (PositionData)character.Position, 
										Destination = (PositionData)character.Destination,
										Moving = character.Moving
									}, ClientParameterCode.Object);
		}
	}
}

