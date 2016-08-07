using System;
using System.Collections.Generic;
using RegionServer.Model.Stats;

namespace RegionServer.Model.Effects.Definitions
{
    public class HealEffect : IEffect
    {
        public byte Duration { get; set; }
        public byte Rank { get; set; }
        public Dictionary<Type, float> statAdd { get; set; }
        public Dictionary<Type, float> statMultiply { get; set; }
        public EffectType Type { get; set; }
        public EffectEnum EnumId { get; set; }

        public HealEffect()
        {
            Duration = 0;
            statAdd = new Dictionary<Type, float>();
            statMultiply = new Dictionary<Type, float>();
            Type = EffectType.STATONLY;
            EnumId = EffectEnum.HEAL;
        }


        public void OnApply(CCharacter owner)
        {
            owner.Stats.ApplyHeal((int)statAdd[new CurrHealth().GetType()]);
        }

        public void OnUpdate()
        {
        }

        public void AddStats()
        {
            statAdd.Add(new CurrHealth().GetType(), 100);
        }

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
