using System.Collections.Generic;
using SubServerCommon;
using ComplexServerCommon;
using SubServerCommon.MethodExtensions;

namespace RegionServer.Model.Constants
{
    public class RegionConstants
    {
        public static readonly Dictionary<ConstantType, Dictionary<byte, int>> Values;

        static RegionConstants()
        {
            Values = new Dictionary<ConstantType, Dictionary<byte, int>>();
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var regionConstants = session.QueryOver<SubServerCommon.Data.NHibernate.Constants>().SingleOrDefault();
                    var stats = SerializeUtil.Deserialize<Dictionary<byte, int>>(regionConstants.statsJson);
                    var currency = SerializeUtil.Deserialize<Dictionary<byte, int>>(regionConstants.currencyJson);
                    var experience = SerializeUtil.Deserialize<Dictionary<byte, int>>(regionConstants.experienceLevelJson);
                    Values.Add(ConstantType.STAT_POINTS_PER_LEVEL, stats);
                    Values.Add(ConstantType.CURRENCY_PER_LEVEL, currency);
                    Values.Add(ConstantType.EXPERIENCE_FOR_LEVEL, experience);
                    transaction.Commit();
                }
            }
        }

        public static Dictionary<byte, int> GetConstants(ConstantType type)
        {
            return Values[type];
        }
    }
}
