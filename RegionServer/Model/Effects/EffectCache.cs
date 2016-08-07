using System.Collections.Generic;
using RegionServer.Model.Effects.Definitions;

namespace RegionServer.Model.Effects
{
    public class EffectCache
    {
        private static readonly Dictionary<EffectEnum, IEffect> _allEffects = new Dictionary<EffectEnum, IEffect>();

        public EffectCache(IEnumerable<IEffect> effects)
        {
            foreach (var effect in effects)
            {
                if (effect is IEffectSpell) continue;
                _allEffects.Add(effect.EnumId, effect);
            }
        }

        public static IEffect GetEffect(EffectEnum name)
        {
            IEffect result;

            if (_allEffects.TryGetValue(name, out result))
            {
                return result;
            }

            //return an empty IEffect if passing null produces no errors. It shouldn't since all IEffects are injected via Autofac
            return null; 
        }
    }
}
