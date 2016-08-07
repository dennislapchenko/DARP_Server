using System;
using System.Collections.Generic;
using NHibernate.SqlCommand;
using RegionServer.Model.Interfaces;

namespace RegionServer.Model.Effects.Definitions
{
    public interface IEffect
    {

        byte Duration { get; set; }
        byte Rank { get; set; } 
        Dictionary<Type, float> statAdd { get; set; }
        Dictionary<Type, float> statMultiply { get; set; }
        void AddStats();
        EffectType Type { get; }
        EffectEnum EnumId { get; }

        void OnApply(CCharacter owner);
        void OnUpdate();
        void OnFinish();
        void OnDamageTaken(int damage, CCharacter attaker);
        void OnDamageGiven(int damage, CCharacter target);
    }
}
