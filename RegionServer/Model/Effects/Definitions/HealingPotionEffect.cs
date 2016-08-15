using System;
using System.Collections.Generic;
using RegionServer.Model.Stats;

namespace RegionServer.Model.Effects.Definitions
{
    public class HealingPotionEffect : IEffect
    {
        public string Name { get { return "Healing Potion"; } }
        public string Description { get { return "Consuming the potion will restore some health"; } }
        public byte Duration { get; set; }
        public byte Rank { get; set; }
        public Dictionary<Type, StatBonus> StatBonuses { get; set; }
        public EffectEnum EnumId { get; }
        public IEffect SecondaryEffect { get; }

        public HealingPotionEffect()
        {
            Duration = 0;
        }

        public IEffect Clone()
        {
            return new HealingPotionEffect() {StatBonuses = StatBonuses};
        }

        public void OnApply(CCharacter owner)
        {
            owner.Stats.ApplyHeal(StatBonuses[typeof (CurrHealth)].AdditiveBonus);
        }

        public void OnUpdate()
        {
        }

        public void AddStats()
        {
            StatBonuses = new Dictionary<Type, StatBonus>
                {
                    {typeof (CurrHealth), StatBonus.New(250, 0.0f)}
                };
        }

        public EffectType Type { get; }

        public void OnFinish()
        {
        }

        public void OnDamageTaken(int damage, CCharacter attacker)
        {
        }

        public void OnDamageGiven(int damage, CCharacter target)
        {
        }
    }
}
