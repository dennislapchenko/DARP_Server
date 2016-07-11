using System.Collections.Generic;

namespace ComplexServerCommon.MessageObjects
{
    [System.Serializable]
    public class InventoryItem
    {
        Dictionary<int, ClientItemInfo> Inventory;
        List<ClientItemInfo> Equipment;

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

