using System;
using System.Collections.Generic;
using NHibernate.Util;
using RegionServer.Model.Stats;
using RegionServer.Model.Stats.PrimaryStats;

namespace RegionServer.Model.Effects.Definitions
{
    class InjuryEffect : IEffect
    {
        public string Name { get { return "Ijury";} }
        public string Description { get { return "Injury causes character's main attributes be impaired"; } }
        public byte Duration { get; set; }
        public byte Rank { get; set; }
        public Dictionary<Type, StatBonus> StatBonuses { get; set; }

        public void AddStats()
        {
            Rank = (byte)RngUtil.intMax(2);
            Duration = new byte[] { 5, 12, 35 }[Rank];

            StatBonuses = new Dictionary<Type, StatBonus>
                                            {
                                                {typeof (Strength), StatBonus.New(0,-0.6f)},
                                                {typeof (Dexterity), StatBonus.New(0,-0.6f)},
                                                {typeof (Instinct), StatBonus.New(0,-0.6f)},
                                                {typeof (Stamina), StatBonus.New(0,-0.6f)},
                                                {typeof (MaxHealth), StatBonus.New(0,-0.6f)}
                                            };
            StatBonuses.ForEach(x => x.Value.Flip());
        }

        public EffectType Type { get {return EffectType.STATONLY;} }
        public EffectEnum EnumId { get { return EffectEnum.INJURY; } }
        public IEffect SecondaryEffect { get { return null;} }

        public IEffect Clone()
        {
            return  new InjuryEffect() { StatBonuses = StatBonuses };
        }

        public void OnApply(CCharacter owner)
        {
            owner.Stats.RefreshCurrentHealth(); //TODO: this should be done in StatHolder upon any maxHealth downward changes
        }

        public void OnUpdate()
        {
            //duration-- is auto done after this call in EffectHolder 
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
