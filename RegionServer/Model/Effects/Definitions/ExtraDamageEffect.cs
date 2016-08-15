using System;
using System.Collections.Generic;
using System.Linq;
using RegionServer.Model.Stats;

namespace RegionServer.Model.Effects.Definitions
{
    public class ExtraDamageEffect : IEffectSpell
    {
        string IEffect.Name { get { return "Extra Damage"; } }
        string IEffect.Description { get { return String.Format("Increases damage of next attack!"); }}
        public byte Duration { get; set; }
        public byte Rank { get; set; }
        public Dictionary<Type, StatBonus> StatBonuses { get; set; }
        public EffectType Type { get {return EffectType.STATONLY;} }
        public EffectEnum EnumId { get {return EffectEnum.DAMAGE;} }

        public byte UnlockLevel { get { return 0; } }
        public IEffect SecondaryEffect { get; }

        public IEffect Clone()
        {
            return new ExtraDamageEffect() { StatBonuses = StatBonuses };
        }

        public void OnApply(CCharacter owner)
        {
            Duration = 1;
        }

        public void OnUpdate()
        {
        }

        public void OnFinish()
        {
            DebugUtils.Logp(String.Format("Spell {0} is fading away", ((IEffect)this).Name));
        }

        public void AddStats()
        {
            StatBonuses = new Dictionary<Type, StatBonus>()
                 {
                    {typeof(MinDamage), StatBonus.New(0, 0.4f)},
                    {typeof(MaxDamage), StatBonus.New(0, 0.4f)}
                };
        }

        public void OnDamageTaken(int damage, CCharacter attaker)
        {
        }

        public void OnDamageGiven(int damage, CCharacter target)
        {
        }
    }
}
