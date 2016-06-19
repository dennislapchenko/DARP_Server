using System;

namespace ComplexServerCommon.MessageObjects
{
	[Serializable]
	public class GenStatData
	{
		public string Name {get;set;}
		public int Experience{get; set;}

		public int Battles{get;set;}
		public int Win {get; set;}
		public int Loss {get; set;}
		public int Tie {get; set;}

		public int Gold {get; set;}
		public int Skulls {get; set;}

		public int InventorySlots {get;set;}

		public GenStatData()
		{
		}
		public GenStatData(string name, int exp, int battles, int win, int loss, int tie, int gold, int skulls, int invSlots)
		{
			Name = name;
			Experience = exp;
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

