using System.Collections.Generic;
using ComplexServerCommon.Enums;

namespace ComplexServerCommon.MessageObjects
{
    [System.Serializable]
    public class FightQueueListItem
    {
        public string FightId;
        public FightType Type;
        public string Creator; //to add creator straight into a team
        public List<string> Blue;
        public List<string> Red;
        public float Timeout; //time after which the attack turn expires and is automatically calculated
        public int TeamSize;
        public bool Sanguinary;  //can or cannot receive time-based, stat-lowering injury if defeated
    }
}

