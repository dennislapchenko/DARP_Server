using System;
using System.Collections.Generic;

namespace ComplexServerCommon.MessageObjects
{
	[Serializable]
	public class InventoryItem
	{
		Dictionary<int, ClientItemInfo> Inventory {get;set;}
		List<ClientItemInfo> Equipment {get;set;}

//		public InventoryItem(Dictionary<int, IStat> inv)
//		{
//			Inventory = inv;
//		}
		public InventoryItem()
		{
		}
	}

	public struct ClientItemInfo
	{
		int itemId;
	}
}

