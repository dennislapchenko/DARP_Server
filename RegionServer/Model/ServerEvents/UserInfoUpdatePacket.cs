using ComplexServerCommon;
using ComplexServerCommon.MessageObjects;
using System.Linq;
using RegionServer.Model.CharacterDatas;

namespace RegionServer.Model.ServerEvents
{
	public class UserInfoUpdatePacket : ServerPacket
	{
		public UserInfoUpdatePacket(CPlayerInstance player) : base(ClientEventCode.ServerPacket, MessageSubCode.UserInfo) 
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
				GenStats = player.GetCharData<GeneralStats>(),

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

