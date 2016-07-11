using System;
using ComplexServerCommon.Enums;

namespace SubServerCommon.Data.NHibernate
{
    public class FightHistory
    {
        public virtual DateTime QueueCreatedTime { get; set; }
        public virtual DateTime FightStartedTime { get; set; }
        public virtual DateTime FightEndedTime { get; set; }
        public virtual string FightId { get; set; }
        public virtual string FightType { get; set; }
        public virtual int TeamSize { get; set; }
        public virtual string Location { get; set; }
        public virtual long FightDuration { get; set; }
        public virtual int MovesExchanged { get; set; }
        public virtual string TeamRedNames { get; set; }
        public virtual string TeamBlueNames { get; set; }
        public virtual string HighestDamagePlayer { get; set; }
        public virtual string LowestDamagePlayer { get; set; }
    }
}
