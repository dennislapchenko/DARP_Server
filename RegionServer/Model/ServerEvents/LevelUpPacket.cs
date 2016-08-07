using ComplexServerCommon;
using ComplexServerCommon.MessageObjects;
using RegionServer.Model.CharacterDatas;
using RegionServer.Model.Stats;
using RegionServer.Model.Stats.BaseStats;

namespace RegionServer.Model.ServerEvents
{
    public class LevelUpPacket : ServerPacket
    {
        public LevelUpPacket(CCharacter instance, bool sendToSelf = true) 
            : base(ClientEventCode.ServerPacket, MessageSubCode.LevelUp, sendToSelf)
        {
            var data = new LevelUpData()
                                    {
                                        NewLevel = (byte)instance.Stats.GetStat<Level>(),
                                        NewNextLevelExperience = instance.GetCharData<GeneralStats>().NextLevelExperience,
                                        CurrentGold = instance.GetCharData<GeneralStats>().Gold,
                                        StatPoints = (byte)instance.Stats.GetStat<StatPoints>()
                                    };
            AddSerializedParameter(data, ClientParameterCode.Object);
        }
    }
}

//LevelUp Data
//public byte NewLevel;
//public int NewNextLevelExperience;
//public int CurrentGold;
//public short CurrentSkulls;
//public byte StatPoints;
//public byte NewSpellId;
