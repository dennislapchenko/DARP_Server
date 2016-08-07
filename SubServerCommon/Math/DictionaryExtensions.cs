using System.Collections.Generic;
using ExitGames.Logging;
using NHibernate.Util;

namespace SubServerCommon.Math
{
    public static class DictionaryExtensions
    {
        private static ILogger Log = LogManager.GetCurrentClassLogger();

        public static void LogPAllElements<K, V>(this Dictionary<K,V> r, string name)
        {
            r.ForEach(kv => Log.DebugFormat("{0}:: K: {1} / V: {2}", name, kv.Key, kv.Value));
        }
    }
}
