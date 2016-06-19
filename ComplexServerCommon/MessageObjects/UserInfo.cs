using System;
using System.Collections.Generic;
using System.Collections;
using System.Xml.Serialization;

namespace ComplexServerCommon.MessageObjects
{
	[Serializable]
	public class UserInfo
	{
		public PositionData Position {get; set;}
		public string Name {get; set;}
		public GenStatData GenStats {get;set;}
		public int[] EquipmentKeys {get; set;}
		public ItemData[] EquipmentValues {get; set;}
		public int[] InventoryKeys {get; set;}
		public ItemData[] InventoryValues {get;set;}
		[XmlArray(ElementName="StatsKVPairs")]
		public List<KeyValuePairS<string, float>> Stats{get;set;}
	}
}

