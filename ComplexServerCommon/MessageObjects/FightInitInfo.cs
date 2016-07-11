using System.Collections.Generic;
using ComplexServerCommon.Enums;

namespace ComplexServerCommon.MessageObjects
{
    [System.Serializable]
    public class FightInitInfo //contains name, fight visible stats, equipment, team
    {
        public FightType fightType;
        public bool sanguinary;
        public Dictionary<int, CharFightInfo> allChars; //key - objectId
        public Dictionary<int, ExchangeProfile> moveLog;

		public FightInitInfo(FightType type, bool sang, Dictionary<int, CharFightInfo> charInfos)
		{
			fightType = type;
			sanguinary = sang;
			allChars = charInfos;
		}
		public FightInitInfo()
		{}
	}
}

