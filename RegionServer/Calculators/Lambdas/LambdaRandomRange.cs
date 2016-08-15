using RegionServer.Model.Interfaces;
using System;
using RegionServer.Model;

namespace RegionServer.Calculators.Lambdas
{
    public class LambdaRandomRange : ILambda
    {
        private readonly Random _rand;
        private readonly ILambda _min;
        private readonly ILambda _max;

        public LambdaRandomRange(ILambda min, ILambda max)
        {
            _rand = new Random();
            _min = min;
            _max = max;
        }

        #region ILambda implementation
        public float Calculate(Environment env)
        {
            return RngUtil.intRange((int)_min.Calculate(env), (int)_max.Calculate(env));
        }
        #endregion
    }
}
