using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Conventions;
using NHibernate.Util;
using RegionServer.Calculators;
using RegionServer.Model.Effects.Definitions;
using RegionServer.Model.Stats.BaseStats;
using SubServerCommon.Math;

namespace RegionServer.Model.Effects
{
    public class EffectHolder
    {
        public CCharacter Owner { get; set; }

        public Dictionary<EffectType, Dictionary<EffectEnum, IEffect>> EffectLists;
        public Dictionary<EffectEnum, IEffectSpell> SpellList;

        public EffectHolder(IEnumerable<IEffectSpell> effectSpells, IEnumerable<EffectType> types)
        {
            EffectLists = new Dictionary<EffectType, Dictionary<EffectEnum, IEffect>>();
            foreach (var type in Enum.GetValues(typeof(EffectType)).Cast<EffectType>())
            {
                EffectLists.Add(type, new Dictionary<EffectEnum, IEffect>());
            }

            SpellList = new Dictionary<EffectEnum, IEffectSpell>();
            foreach (var spell in effectSpells)
            {
                SpellList.Add(spell.EnumId, spell);
            }
        }

        public void OnApply(SpellEnvironment env)
        {
            var effect = GetSpell(env.CharacterSpell);
            OnApply(effect);
        }  

        public void OnApply(IEffect effect)
        {
            if (!GetAllEffects(false).ContainsKey(effect.EnumId))
            {
                effect.AddStats();
                effect.OnApply(Owner);
                EffectLists[effect.Type].Add(effect.EnumId, effect);
                if(effect.Duration <= 0) effect.OnFinish();
            }
        }

        public void OnUpdate()
        {
            foreach(var effect in GetAllEffects(false).Values)
            {
                effect.OnUpdate();
                effect.Duration--;
                if (effect.Duration <= 0)
                    effect.OnFinish();
            }
        }

        public void OnFinish()
        {
            var origEffects = GetAllEffects(false);
            var allEffects = new Dictionary<EffectEnum, IEffect>(origEffects);
            foreach (var effect in allEffects.Values)
            {
                effect.OnFinish();
                origEffects.Remove(effect.EnumId);
            }
        }

        public void OnDamageTaken(int damage, CCharacter attacker)
        {
            GetAllEffects(false).Values.ForEach(effect => effect.OnDamageTaken(damage, attacker));
        }

        public void OnDamageGiven(int damage, CCharacter target)
        {
            GetAllEffects(false).Values.ForEach(effect => effect.OnDamageGiven(damage, target));
        }

        public void UseSpell(SpellEnvironment env)
        {
            OnApply(env);
        }

        public void UseSpell(int id)
        {
            if (id == 0) return;
            var effect = GetSpell(id);
            OnApply(effect);
        }

        public IEffectSpell GetSpell(int id)
        {
            var enumId = (EffectEnum) id;
            IEffectSpell result;
            SpellList.TryGetValue(enumId, out result);

            if(result != null && isUnlocked(result))
            {
                return result;
            }
            return null;
        }

        private Dictionary<EffectEnum, IEffect> GetAllEffects(bool withSpells)
        {
            var result = new Dictionary<EffectEnum, IEffect>();
            if (EffectLists == null && EffectLists.IsEmpty()) return result;
            foreach (var effectList in EffectLists)
            {
                result = effectList.Value.Union(result).ToDictionary(k => k.Key, v => v.Value);
            }
            if (withSpells) result = result.Union(SpellList.ToDictionary(x=>x.Key, x => (IEffect)x.Value)).ToDictionary(x=>x.Key, x=>x.Value);
            return result;
        }

        private bool isUnlocked(IEffectSpell effect)
        {
            var level = Owner.Stats.GetStatBase(new Level());
            return (level >= effect.UnlockLevel) ? true : false;
        }


        public List<IEffect> GetEffectsFor(Type statType)
        {
            var result = new List<IEffect>();
            foreach (var effect in GetAllEffects(false).Where(v => v.Value.Type == EffectType.STATONLY))
            {
                if (effect.Value.statAdd.ContainsKey(statType))
                {
                    result.Add(effect.Value);
                }
                if (effect.Value.statMultiply.ContainsKey(statType))
                {
                    result.Add(effect.Value);
                    //DebugUtils.Logp("GetEffectsFor", String.Format("adding mult value of {0} to IEffect with type {1}", effect.Value.statAdd.Count, statType));
                }
            }
            return result;
        }
    }
}
