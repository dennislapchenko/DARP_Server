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
			Map(x => x.GenStats).Column("general_stats");
			Map(x => x.Stats).Column("stats"); //VARCHAR(2048) (if binary use a BLOB)
			Map(x => x.Position).Column("position");//VARCHAR(1024)
			Map(x => x.Items).Column("items");
			References(x => x.UserId).Column ("user_id");
			Table ("characters");
		}
	}
}

