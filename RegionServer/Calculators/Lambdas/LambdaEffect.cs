using System;
using System.Linq;
using RegionServer.Model.Effects.Definitions;
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
                foreach (var effect in env.Character.Effects.GetEffectsForStat(_stat.GetType()))
                {
                    StatBonus statBonuses;
                    effect.StatBonuses.TryGetValue(_stat.GetType(), out statBonuses);

                    totalAdd += statBonuses.AdditiveBonus; //in case where no either stat bonus present a default value of int 0 or float 0.0 will be added (aka nothing)
                    totalMultiply += statBonuses.MultiplicativeBonus;
                    //                  DebugUtils.Logp(DebugUtils.Level.INFO, "lambdaEffect", "add from effects", String.Format("lambda stat: {0}, inc effects: {1}-{2}", 
                    //                   _stat.GetType(), effect.statAdd.Count, effect.statMultiply.Count + " and inc type is "+effect.GetType().ToString()));
                    //DebugUtils.Logp("lambda effect add. out?"+ , out newAdd)+" value: ", newAdd+".");
                    //DebugUtils.Logp("\n\nTotal Add post accrue:", totalAdd+" (was applying: "+newAdd+")\n");
                    //DebugUtils.Logp("lambda effect Multiply. out?"+ , out newMult)+" value: ", newMult+".");
                    //DebugUtils.Logp("\n\nTotal Multiply post accrue:", totalMultiply+" (was applying: "+newMult+")\n");
                }
                if(env.Value > 0.0) env.Value = (env.Value * totalMultiply)+totalAdd;
                //DebugUtils.Logp("\n\n", String.Format("Initial env.val:{0}, postAccrue env.val:{1} ornew:{2}", saveValue, env.Value, tryoutValue));
            }

            returnValue = env.Value - saveValue;
            env.Value = saveValue;

            return returnValue;
        }
    }
}
