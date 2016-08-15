using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using SubServerCommon.Data.NHibernate;

namespace SubServerCommon.Data.Mapping
{
    public class ConstantsMap : ClassMap<Constants>
    {
        public ConstantsMap()
        {
            Id(x => x.area).Column("area");
            Map(x => x.statsJson).Column("statpointsperlevel");
            Map(x => x.currencyJson).Column("currencyperlevel");
            Map(x => x.experienceLevelJson).Column("experienceforlevel");
            Table("regionconstants");
        }
    }
}
