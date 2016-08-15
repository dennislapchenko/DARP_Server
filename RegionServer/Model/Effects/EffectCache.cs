using System;
using System.Collections.Generic;
using ComplexServerCommon;
using RegionServer.Model.Effects.Definitions;
using SubServerCommon.MethodExtensions;

namespace RegionServer.Model.Effects
{
    public class EffectCache
    {
        private static readonly Dictionary<EffectEnum, IEffect> _allEffects = new Dictionary<EffectEnum, IEffect>();

        public static EffectCache Instance;

        public EffectCache(IEnumerable<IEffect> effects)
        {
            Instance = this;
            effects.LogAllElements("effectCache-effects");
            foreach (var effect in effects)
            { 
                if (effect is IEffectSpell) continue;
                effect.AddStats();
                _allEffects.Add(effect.EnumId, effect);
            }
        }

        public static IEffect GetEffect(EffectEnum name)
        {
            IEffect result;

            if (_allEffects.TryGetValue(name, out result))
            {
                //DebugUtils.Logp("EffectCache::GetEffect", String.Format("found effect: {0}-{1}", result.EnumId, result.GetType()));
                return result.Clone();
            }

            //return an empty IEffect if passing null produces no errors. It shouldn't since all IEffects are injected via Autofac
            return null; 
        }
    }
}
