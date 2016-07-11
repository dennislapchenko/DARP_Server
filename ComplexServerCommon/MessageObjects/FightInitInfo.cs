using System.Collections.Generic;
using ComplexServerCommon.Enums;

namespace ComplexServerCommon.MessageObjects
{
    [System.Serializable]
    public class FightInitInfo //contains name, fight visible stats, equipment, team
    {
        public FightType fightType;
        public bool sanguinary;
        public List<KeyValuePairS<int, CharFightInfo>> allChars; //key - objectId
        public List<KeyValuePairS<int, ExchangeProfile>> moveLog;

		public FightInitInfo(FightType type, bool sang, List<KeyValuePairS<int, CharFightInfo>> charInfos)
		{
			fightType = type;
			sanguinary = sang;
			allChars = charInfos;
		}
		public FightInitInfo()
		{}
	}
}

