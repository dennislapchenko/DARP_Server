using ComplexServerCommon;
using ComplexServerCommon.MessageObjects;
using RegionServer.Model.Stats;

namespace RegionServer.Model.ServerEvents
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

