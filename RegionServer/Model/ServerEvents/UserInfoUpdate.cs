using ComplexServerCommon;
using ComplexServerCommon.MessageObjects;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using RegionServer.Model.Items;

namespace RegionServer.Model.ServerEvents
{
	public class UserInfoUpdate : ServerPacket
	{
		public UserInfoUpdate(CPlayerInstance player) : base(ClientEventCode.ServerPacket, MessageSubCode.UserInfo) 
		{
			AddParameter(player.ObjectId, ClientParameterCode.ObjectId);
			AddUserInfo(player);
		} 

		private void AddUserInfo(CPlayerInstance player)
		{
			var equipment = player.Items.Equipment.ToDictionary(item => (int)item.Key, item => (ItemData)item.Value);
			var inventory = player.Items.Inventory.ToDictionary(item => item.Key, item => (ItemData)item.Value);
			UserInfo info = new UserInfo() 
			{
				Position = player.Position,
				Name = player.Name,
				GenStats = player.GenStats,

				//Attributes - level, exp, stats
				Stats = player.Stats.GetAllStats(),

				//inventory - all equiped items
				EquipmentKeys = equipment.Keys.ToArray(),
				EquipmentValues = equipment.Values.ToArray(),

				InventoryKeys = inventory.Keys.ToArray(),
				InventoryValues = inventory.Values.ToArray(),


				//Talents - skills

				//effects

				//movement speed
				//action/emotes
			};
			AddSerializedParameter(info, ClientParameterCode.Object, false);
		}
	}
}

