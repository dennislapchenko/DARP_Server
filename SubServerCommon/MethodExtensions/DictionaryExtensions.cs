using System.Collections.Generic;
using ExitGames.Logging;
using NHibernate.Util;

namespace SubServerCommon.MethodExtensions
{
    public static class DictionaryExtensions
    {
        private static ILogger Log = LogManager.GetCurrentClassLogger();

        public static void LogAllElements<K, V>(this Dictionary<K,V> r, string name)
        {
            r.ForEach(kv => Log.DebugFormat("{0}:: K: {1} / V: {2}", name, kv.Key, kv.Value));
        }

        public static void LogAllElements<T>(this IEnumerable<T> r, string name)
        {
            r.ForEach(kv => Log.DebugFormat("{0}:: type: {1}", name, kv.GetType()));
        }
    }
}
