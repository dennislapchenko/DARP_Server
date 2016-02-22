using FluentNHibernate.Mapping;
using SubServerCommon.Data.NHibernate;

namespace SubServerCommon.Data.Mapping
{
	public class ComplexCharacterMap : ClassMap<ComplexCharacter>
	{
		public ComplexCharacterMap ()
		{
			Id(x => x.Id).Column("id");
			Map(x => x.Name).Column("name");
			Map(x => x.Class).Column("class");
			Map(x => x.Level).Column("level");
			Map(x => x.Sex).Column("sex");
			References(x => x.UserId).Column ("user_id");
			Table ("characters");
		}
	}
}

