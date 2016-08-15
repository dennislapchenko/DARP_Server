using ComplexServerCommon;

namespace RegionServer.Model.ServerEvents.CharacterEvents
{
    public class HP5Packet : ServerPacket
    {
        public HP5Packet(int newHealth) : base(ClientEventCode.ServerPacket, MessageSubCode.RegenUpdates)
        {
            AddParameter(newHealth, ClientParameterCode.CurrentHealth);
        }
    }
}