using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FluentNHibernate.Conventions;
using NHibernate.Util;
using RegionServer.Calculators;
using RegionServer.Model.Effects.Definitions;
using RegionServer.Model.ServerEvents.CharacterEvents;
using RegionServer.Model.Stats.BaseStats;

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
                spell.AddStats();
                SpellList.Add(spell.EnumId, spell);
            }
        }

        private List<IEffect> getMainAndSubEffects(IEffect effect, List<IEffect> mainAndSubEffects)
        {
            DebugUtils.nullLog(effect?.SecondaryEffect, "secondary effect");

            if (effect != null && mainAndSubEffects.Count < 3)
            {
                mainAndSubEffects.Add(effect);
                DebugUtils.Logp(String.Format("Recursively gained effect {0} (total fX now: {1})", effect.EnumId, mainAndSubEffects.Count));
                getMainAndSubEffects(effect.SecondaryEffect, mainAndSubEffects);
            }
   
            return mainAndSubEffects;
        }

        public void Apply(IEffect effect)
        {
            if (!EffectLists[effect.Type].ContainsKey(effect.EnumId))
            {
                OnApply(effect);
            }
        }

        private void OnApply(IEffect mainEffect)
        {
            if (mainEffect == null) return;

            List<IEffect> mainAndSubEffects = new List<IEffect>();
            mainAndSubEffects = getMainAndSubEffects(mainEffect, mainAndSubEffects);
            foreach (var effect in mainAndSubEffects)
            {
                if (EffectLists[effect.Type].ContainsKey(effect.EnumId)) continue;
                effect.OnApply(Owner);
                EffectLists[effect.Type].Add(effect.EnumId, effect);
                DebugUtils.Logp(String.Format("Char {0} gained effect {1}", Owner.Name, effect.EnumId));
                if(effect.Duration <= 0) OnFinish(effect);
            }

            var owner = Owner as CPlayerInstance;
            if (owner != null)
            {
                owner.SendPacket(new UserInfoUpdatePacket(owner));
            }
        }

        public void OnUpdate()
        {
            foreach(var effect in GetAllEffects(false).Values)
            {
                effect.OnUpdate();
                if (--effect.Duration <= 0)
                {
                    DebugUtils.Logp(String.Format("{0}:: {1} is fading away", Owner.Name, effect.EnumId));
                    OnFinish(effect);
                }
            }
        }

        public void OnFinish(IEffect effect)
        {
            Debug.Assert(effect.Duration <= 0);
            effect.OnFinish();
            GetAllEffects(false).Remove(effect.EnumId);
        }

        public void OnFinish()
        {
            foreach (var effect in GetAllEffects(false).Values)
            {
                OnFinish(effect);
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
            UseSpell(env.CharacterSpell);
        }

        public void UseSpell(byte id)
        {
            if (id == 0) return;
            var effect = GetSpell(id);
            DebugUtils.Logp(String.Format("(UseSpell(byteId)Using spell {0} (gaining:{1}) to character: {2}", effect.Name, 1, Owner.Name));
            OnApply(effect);
        }

        public IEffectSpell GetSpell(int id)
        {
            var enumId = (EffectEnum) id;
            IEffectSpell result;
            SpellList.TryGetValue(enumId, out result);
            if(result != null && isUnlocked(result))
            {
                DebugUtils.Logp(String.Format("Found an unlocked spell {0}-{1} on char {2}. using it!", result.Name, result.EnumId, Owner.Name));
                return result;
            }
            DebugUtils.Logp(String.Format("NO SPELL WAS FOUND by enumID {0} on char {1}. ERROR!", result.EnumId, Owner.Name));
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
            return result;
        }

        private bool isUnlocked(IEffectSpell effect)
        {
            var level = Owner.Stats.GetStatBase(new Level());
            return (level >= effect.UnlockLevel);
        }


        public List<IEffect> GetEffectsForStat(Type statType)
        {
            var result = new List<IEffect>();
            foreach (var effect in GetAllEffects(false).Where(v => v.Value.Type == EffectType.STATONLY))
            {
                if (effect.Value.StatBonuses.ContainsKey(statType))
                {
                    result.Add(effect.Value);
                }
            }
            return result;
        }
    }
}
