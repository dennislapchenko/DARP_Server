using NHibernate.Mapping.ByCode;
using RegionServer.Model.Interfaces;

namespace RegionServer.Calculators.Lambdas
{
    public class LambdaStatMapToRange : ILambda
    {
        private readonly IStat _stat;
        private readonly float _imin;
        private readonly float _imax;
        private readonly float _omin;
        private readonly float _omax;

        private readonly bool _useTarget;

        public LambdaStatMapToRange(IStat stat, float imin, float imax, float omin, float omax, bool useTarget = false)
        {
            _stat = stat;
            _imin = imin;
            _imax = imax;
            _omin = omin;
            _omax = omax;
            _useTarget = useTarget;
        }

        #region ILambda implementation
        public float Calculate(Environment env)
        {
            if ((_useTarget && env.Target == null) || (!_useTarget && env.Character == null))
            {
                return 1.0f;
            }
            if (_useTarget)
            {
                return (float)Util.MapToRange(env.Target.Stats.GetStat(_stat), _imin, _imax, _omin, _omax);
            }

            return (float)Util.MapToRange(env.Character.Stats.GetStat(_stat), _imin, _imax, _omin, _omax);
        }
        #endregion
    }
}