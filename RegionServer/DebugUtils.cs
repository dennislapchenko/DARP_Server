using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExitGames.Logging;
using NHibernate.Properties;

namespace RegionServer
{
	class DebugUtils
	{
		protected static ILogger Log = LogManager.GetCurrentClassLogger();

        public enum Level { INFO, ERROR, WARNING, FATAL};

		public static void nullLog(string className, string methodName, Object obj, string objName)
		{
			Log.DebugFormat(obj == null ? "{0}::{1} -- {2} is NULL" : "{0}::{1} -- {2} is NOT NULL", className, methodName, objName);
		}

		public static void nullLog(string className, string methodName, Object[] objs, string[] objNames)
		{
			int index = 0;
			foreach (var obj in objs)
			{
				Log.DebugFormat(obj == null ? "{0}::{1} -- {2} is NULL" : "{0}::{1} -- {2} is NOT NULL", className, methodName, objNames[index++]);
			}
		}

	    public static void Logp(Level severity, string classname, string methodname, string message)
	    {
	        switch (severity)
	        {
                case (Level.INFO):
                    Log.DebugFormat("{0}::{1} -- {2}", classname, methodname, message);
                    break;
                case (Level.ERROR):
                    Log.ErrorFormat("{0}::{1} -- {2}", classname, methodname, message);
                    break;
                case (Level.WARNING):
                    Log.WarnFormat("{0}::{1} -- {2}", classname, methodname, message);
                    break;
                case (Level.FATAL):
                    Log.FatalFormat("{0}::{1} -- {2}", classname, methodname, message);
                    break;
	        }
	    }
	}
}
