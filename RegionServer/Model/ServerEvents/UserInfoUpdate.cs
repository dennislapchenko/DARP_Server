using ComplexServerCommon;
using ComplexServerCommon.MessageObjects;

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
				//when initialize character (on login)
				Position = player.Position,
				Name = player.Name

				//Attributes - level, exp, stats

				//inventory - all equiped items

				//Talents - skills

				//effects

				//movement speed
				//action/emotes
			};
			AddSerializedParameter(info, ClientParameterCode.Object, false);
		}
	}
}

