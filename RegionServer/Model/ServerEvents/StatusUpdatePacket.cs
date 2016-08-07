using RegionServer.Model.ServerEvents;
using ComplexServerCommon;


namespace RegionServer.Model.ServerEvents
{
	public class StatusUpdatePacket : ServerPacket
	{
		public StatusUpdatePacket(CCharacter character) 
			: base (ClientEventCode.ServerPacket, MessageSubCode.StatusUpdate)
		{
			AddParameter(character.ObjectId, ClientParameterCode.ObjectId);
		}
	}
}

