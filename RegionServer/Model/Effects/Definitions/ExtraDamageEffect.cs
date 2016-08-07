using System;
using System.Collections.Generic;
using System.Linq;
using RegionServer.Model.Stats;

namespace RegionServer.Model.Effects.Definitions
{
    public class ExtraDamageEffect : IEffectSpell
    {
        public byte Duration { get; set; }
        public byte Rank { get; set; }
        public Dictionary<Type, float> statAdd { get; set; }
        public Dictionary<Type, float> statMultiply { get; set; }
        public EffectType Type { get {return EffectType.STATONLY;} }
        public EffectEnum EnumId { get {return EffectEnum.DAMAGE;} }

        public ExtraDamageEffect()
        {
            Duration = 1;
            statAdd = new Dictionary<Type, float>();
            statMultiply = new Dictionary<Type, float>();
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

        public void AddStats()
        {
            statMultiply.Add(new MinDamage().GetType(), 10.4f);
            statMultiply.Add(new MaxDamage().GetType(), 10.4f);
        }

        public void OnDamageTaken(int damage, CCharacter attaker)
        {
        }

        public void OnDamageGiven(int damage, CCharacter target)
        {
        }

        public string Name { get { return "Extra Damage"; } }
        public string Description { get { return String.Format("Increases damage of next attack by {0}", statMultiply.FirstOrDefault().Value); }}
        public byte UnlockLevel { get { return 0; } }
    }
}
