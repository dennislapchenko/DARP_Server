using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace ComplexServerCommon.MessageObjects
{
	[Serializable]
	public class ItemData
	{
		public string Name {get; set;}
		public int ItemId {get; set;}
		public int Type {get; set;}
		public int Slot {get; set;}
		public int Value {get; set;}
		public int Equippable {get; set;}
		public int LevelReq {get; set;}
		public List<KeyValuePairS<string, float>> Stats {get;set;}
		//public int Quality {get;set;}

		public ItemData()
		{
		}

		public ItemData(string name, int itemId, int type, int slot, int value, int equippable, int levelReq, List<KeyValuePairS<string, float>> stats)
		{
			Name = name;
			ItemId = itemId;
			Type = type;
			Slot = slot;
			Value = value;
			Equippable = equippable;
			LevelReq = levelReq;
			Stats = stats;
		}
	}
}

