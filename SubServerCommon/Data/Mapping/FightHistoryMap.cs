using System.Linq;
using System.Security.Cryptography.X509Certificates;
using FluentNHibernate.Mapping;
using SubServerCommon.Data.NHibernate;

namespace SubServerCommon.Data.Mapping
{
    public class FightHistoryMap : ClassMap<FightHistory>
    {
        public FightHistoryMap()
        {
            Id(x => x.FightId).Column("FIGHT_ID");
            Map(x => x.FightType).Column("TYPE");
            Map(x => x.TeamSize).Column("TEAMSIZE");
            Map(x => x.TeamRedNames).Column("REDNAMES");
            Map(x => x.TeamBlueNames).Column("BLUENAMES");
            Map(x => x.Location).Column("LOCATION");
            Map(x => x.QueueCreatedTime).Column("QUEUE_CREATED");
            Map(x => x.FightStartedTime).Column("FIGHT_STARTED");
            Map(x => x.FightEndedTime).Column("FIGHT_ENDED");
            Map(x => x.FightDuration).Column("DURATION");
            Map(x => x.MovesExchanged).Column("MOVES");
            Map(x => x.LowestDamagePlayer).Column("LOWEST_DAMAGE");
            Map(x => x.HighestDamagePlayer).Column("HIGHEST_DAMAGE");
            Table("fighthistory");
        }
    }
}