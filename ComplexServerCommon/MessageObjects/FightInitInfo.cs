using System;
using System.Collections.Generic;
using ComplexServerCommon.Enums;

namespace ComplexServerCommon.MessageObjects
{
	[Serializable]
	public class FightInitInfo //contains name, fight visible stats, equipment, team
	{
		public FightType fightType {get;set;}
		public bool sanguinary {get;set;}
		public List<KeyValuePairS<int, CharFightInfo>> allChars {get;set;} //key - objectId
		public List<KeyValuePairS<int, ExchangeProfile>> moveLog {get;set;}

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

