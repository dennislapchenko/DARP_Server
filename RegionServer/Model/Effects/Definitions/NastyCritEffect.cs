using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RegionServer.Model.Stats.SecondaryStats;

namespace RegionServer.Model.Effects.Definitions
{
    public class NastyCritEffect : IEffect
    {
        public string Name { get { return "Nasty Critical Hit";} }
        public string Description { get { return "Git Rekt son!"; } }
        public byte Duration { get; set; }
        public byte Rank { get; set; }
        public Dictionary<Type, StatBonus> StatBonuses { get; set; }

        public void AddStats()
        {
            StatBonuses = new Dictionary<Type, StatBonus>()
            {
                {typeof (CriticalHitChance), StatBonus.New(100,0.0f)},
                {typeof (CriticalDamage), StatBonus.New(40, 0.0f)},
            };
        }

        public EffectType Type { get {return EffectType.STATONLY;} }
        public EffectEnum EnumId { get {return EffectEnum.NASTY_CRIT;} }
        public IEffect SecondaryEffect { get; }
        public IEffect Clone()
        {
            return new NastyCritEffect {StatBonuses = StatBonuses};
        }

        public void OnApply(CCharacter owner)
        {
        }

        public void OnUpdate()
        {
        }

        public void OnFinish()
        {
        }

        public void OnDamageTaken(int damage, CCharacter attaker)
        {
        }

        public void OnDamageGiven(int damage, CCharacter target)
        {
        }
    }
}
