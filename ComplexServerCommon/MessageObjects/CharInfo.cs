using System;
using System.Collections.Generic;


namespace ComplexServerCommon.MessageObjects
{
	[Serializable]
	public class CharInfo
	{
		//anything others need to know about character on update
		public PositionData Position {get; set;}
		public int ObjectId {get;set;}
		public string Name {get; set;}
		public GenStatData GenStats {get;set;}
		public List<KeyValuePairS<string, float>> Stats{get;set;}
		public int[] EquipmentKeys {get; set;}
		public ItemData[] EquipmentValues {get; set;}

		public CharInfo()
		{
			
		}
	}
}

