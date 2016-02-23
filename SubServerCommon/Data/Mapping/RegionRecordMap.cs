using SubServerCommon.Data.NHibernate;
using FluentNHibernate.Mapping;
using System.Linq;

namespace SubServerCommon.Data.Mapping
{
	public class RegionRecordMap : ClassMap<RegionRecord>
	{
		public RegionRecordMap()
		{
			Id(x => x.Id).Column("id");
			Map(x => x.Name).Column("name");
			Map(x => x.ColliderPath).Column("collider_path");
			Table("region");
		}
	}
}

