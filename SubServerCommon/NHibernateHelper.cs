
using NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SubServerCommon
{
	public class NHibernateHelper
	{
		public NHibernateHelper()
		{
			InitializeSessionFactory();
		}

		public static List<string> sqlsetup = new List<string>();

		private static ISessionFactory _sessionFactory;
		private static ISessionFactory SessionFactory
		{
			get
			{
				if (_sessionFactory == null)
				{
					InitializeSessionFactory();
				}

				return _sessionFactory;
			}
		}


		private static void InitializeSessionFactory()
		{
			string FilePath = Path.Combine(@"C:\PHOTONSDK\deploy\ComplexServer\esquel.dar");
			sqlsetup = File.ReadLines(FilePath).ToList();
			_sessionFactory = Fluently.Configure().Database(
			MySQLConfiguration.Standard
				.ConnectionString(cs => cs.Server(sqlsetup[0])
			                  .Database(sqlsetup[1])
			                  .Username(sqlsetup[2])
			                  .Password(sqlsetup[3])))
				.Mappings(m => m.FluentMappings.AddFromAssemblyOf<NHibernateHelper>())
				.BuildSessionFactory();
		}

		public static ISession OpenSession()
		{
			return SessionFactory.OpenSession();
		}
	}
}

