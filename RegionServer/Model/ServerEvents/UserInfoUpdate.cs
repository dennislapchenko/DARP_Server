using ComplexServerCommon;
using ComplexServerCommon.MessageObjects;
using System.Linq;

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
			UserInfo info = new UserInfo() 
			{
				Position = player.Position,
				Name = player.Name,
				GenStats = player.GenStats,

				//Attributes - level, exp, stats
				Stats = player.Stats.GetAllStats(),

				//inventory - all equiped items
				Equipment = player.Items.Equipment.ToDictionary(item => (int)item.Key, item => (ItemData)item.Value),
                Inventory = player.Items.Inventory.ToDictionary(item => item.Key, item => (ItemData)item.Value),

            //Talents - skills

            //effects

            //movement speed
            //action/emotes
        };
			AddSerializedParameter(info, ClientParameterCode.Object, false);
		}
	}
}

