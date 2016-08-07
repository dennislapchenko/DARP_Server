using System;
using System.Linq;
using RegionServer.Model.Interfaces;
using SubServerCommon.Math;

namespace RegionServer.Calculators.Lambdas
{
    public class LambdaEffect : ILambda
    {
        private readonly IStat _stat;

        public LambdaEffect(IStat stat)
        {
            _stat = stat;
        }

        public float Calculate(Environment env)
        {
            float saveValue = env.Value;
            float returnValue = 0;

            if (env.Character != null)
            {
                int totalAdd = 0;
                float totalMultiply = 1.0f;
                foreach (var effect in env.Character.Effects.GetEffectsFor(_stat.GetType()))
                {
                    DebugUtils.Logp(DebugUtils.Level.INFO, "lambdaEffect", "add from effects", String.Format("lambda stat: {0}, inc effects: {1}-{2}", 
                        _stat.GetType(), effect.statAdd.Count, effect.statMultiply.Count + " and inc type is "+effect.GetType().ToString()));
                    foreach (var kv in effect.statAdd)
                    {
                        DebugUtils.Logp(DebugUtils.Level.INFO, "lambdaEffect", "statAdd types in effects", String.Format("{0}", kv.Key));
                    }
                    foreach (var kv in effect.statMultiply)
                    {
                        DebugUtils.Logp(DebugUtils.Level.INFO, "lambdaEffect", "statMultiply types in effects", String.Format("{0}", kv.Key));
                    }

                    float newAdd, newMult;
                    effect.statAdd.LogPAllElements("effect add in lambdaeffect");
                    DebugUtils.Logp("lambda effect add. out?"+ effect.statAdd.TryGetValue(_stat.GetType(), out newAdd)+" value: ", newAdd+".");
                    totalAdd += (int) newAdd;
                    //DebugUtils.Logp("\n\nTotal Add post accrue:", totalAdd+" (was applying: "+newAdd+")\n");
                    effect.statMultiply.LogPAllElements("effect multiply in lambdaeffect");
                    DebugUtils.Logp("lambda effect Multiply. out?"+ effect.statMultiply.TryGetValue(_stat.GetType(), out newMult)+" value: ", newMult+".");
                    totalMultiply += newMult;
                    //DebugUtils.Logp("\n\nTotal Multiply post accrue:", totalMultiply+" (was applying: "+newMult+")\n");
                }
                env.Value = (env.Value * totalMultiply)+totalAdd;
                var tryoutValue = (env.Value * totalMultiply) + totalAdd;
                DebugUtils.Logp("\n\n", String.Format("Initial env.val:{0}, postAccrue env.val:{1} ornew:{2}", saveValue, env.Value, tryoutValue));
            }

            returnValue = env.Value;
            env.Value = saveValue;

            return returnValue;
        }
    }
}
