using System.Collections.Generic;
using ComplexServerCommon.Enums;

namespace ComplexServerCommon.MessageObjects
{
    [System.Serializable]
    public class FightCharsInfo
    {
        public FightType Type;
        public bool Sanguinity;
        public List<CharInfo> RedInfo;
        public List<CharInfo> BlueInfo;
        public string UsersTeam;
    }
}

