using System.Linq;
using ComplexServerCommon;
using ComplexServerCommon.MessageObjects;
using RegionServer.Model.CharacterDatas;
using RegionServer.Model.Items;

namespace RegionServer.Model.ServerEvents.CharacterEvents
{
    public class UserStatsUpdatePacket : ServerPacket
    {
        public UserStatsUpdatePacket(CCharacter player) : base(ClientEventCode.ServerPacket, MessageSubCode.UserStatInfo) 
		{
            AddParameter(player.ObjectId, ClientParameterCode.ObjectId);
            AddUserInfo(player);
        }

        private void AddUserInfo(CCharacter player)
        {
            UserInfo info = new UserInfo()
            {
//                Position = player.Position,
//                Name = player.Name,
                GenStats = player.GetCharData<GeneralStats>(),
//
//                //Attributes - level, exp, stats
                Stats = player.Stats.GetAllStats(),
//
//                //inventory - all equiped items
//                Equipment = player.Items.Equipment.ToDictionary(item => (int)item.Key, item => (ItemData)item.Value),
                Inventory = player.Items.Inventory.ToDictionary(item => item.Key, item => (ItemData)(Item)item.Value),
//
//                //Talents - skills
//
//                //effects
//
//                //movement speed
//                //action/emotes
            };
            AddSerializedParameter(info, ClientParameterCode.Object, false);
        }
}
}
