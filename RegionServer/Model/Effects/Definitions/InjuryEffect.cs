using System;
using System.Collections.Generic;
using RegionServer.Model.Stats;
using RegionServer.Model.Stats.PrimaryStats;

namespace RegionServer.Model.Effects.Definitions
{
    class InjuryEffect : IEffect
    {
        public byte Duration { get; set; }
        public byte Rank { get; set; }
        public Dictionary<Type, float> statAdd { get; set; }
        public Dictionary<Type, float> statMultiply { get; set; }

        public void AddStats()
        {
            statAdd = new Dictionary<Type, float>();
            statMultiply = new Dictionary<Type, float>();
            statMultiply.Add(typeof(Strength), -0.6f);
            statMultiply.Add(typeof(Dexterity), -0.6f);
            statMultiply.Add(typeof(Instinct), -0.6f);
            statMultiply.Add(typeof(Stamina), -0.6f);
            statMultiply.Add(typeof(MaxHealth), -0.6f);
        }

        public EffectType Type { get {return EffectType.STATONLY;} }
        public EffectEnum EnumId { get { return EffectEnum.INJURY; } }

        public void OnApply(CCharacter owner)
        {
            Rank = (byte)RngUtil.intMax(2);
            Duration = new byte[]{10, 19, 35}[Rank];
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
