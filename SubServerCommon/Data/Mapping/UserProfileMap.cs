using FluentNHibernate.Mapping;
using SubServerCommon.Data.NHibernate;


namespace SubServerCommon.Data.Mapping
{
	public class UserProfileMap : ClassMap<UserProfile>
	{
		public UserProfileMap()
		{
			Id (x => x.Id).Column("id");
			Map(x => x.CharacterSlots).Column("character_slots");
			References(x => x.UserId).Column ("user_id");
			Table("user_profile");
		}
	}
}

