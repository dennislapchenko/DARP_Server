using System;
using System.Collections.Generic;

namespace RegionServer.Model.Effects.Definitions
{
    public interface IEffect
    {
        string Name { get; }
        string Description { get; }
        byte Duration { get; set; }
        byte Rank { get; set; } 
        Dictionary<Type, StatBonus> StatBonuses { get; set; }
        void AddStats();
        EffectType Type { get; }
        EffectEnum EnumId { get; }
        IEffect SecondaryEffect { get; }

        IEffect Clone();

        void OnApply(CCharacter owner);
        void OnUpdate();
        void OnFinish();
        void OnDamageTaken(int damage, CCharacter attaker);
        void OnDamageGiven(int damage, CCharacter target);
    }
}
