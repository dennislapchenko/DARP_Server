using RegionServer.Model.ServerEvents;
using ComplexServerCommon;


namespace RegionServer.Model.ServerEvents
{
	public class StatusUpdate : ServerPacket
	{
		public StatusUpdate(CCharacter character) 
			: base (ClientEventCode.ServerPacket, MessageSubCode.StatusUpdate)
		{
			AddParameter(character.ObjectId, ClientParameterCode.ObjectId);
		}
	}
}

