using ComplexServerCommon.Enums;

namespace ComplexServerCommon.MessageObjects
{
    [System.Serializable]
    public class Rewards
	{
	    public int TotalDamage;
	    public int Experience;
	    public int Gold;
	    public int Skulls;
	    public ItemData Item; //possibly a list
	    public FightWinLossTie WLT;
	    public bool Injury; //best reward ever <3
	}
}

