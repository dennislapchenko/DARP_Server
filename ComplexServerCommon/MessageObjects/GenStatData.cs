namespace ComplexServerCommon.MessageObjects
{
    [System.Serializable]
    public class GenStatData
    {
        public string Name;
        public int Experience;
        public int NextLevelExperience;

        public int Battles;
        public int Win;
        public int Loss;
        public int Tie;

        public int Gold;
        public int Skulls;

        public int InventorySlots;

		public GenStatData()
		{
		}

		public GenStatData(string name, int exp, int nextLvlExp, int battles, int win, int loss, int tie, int gold, int skulls, int invSlots)
		{
			Name = name;
			Experience = exp;
		    NextLevelExperience = nextLvlExp;
			Battles = battles;
			Win = win;
			Loss = loss;
			Tie = tie;
			Gold = gold;
			Skulls = skulls;
			InventorySlots = invSlots;
		}
	}
}

