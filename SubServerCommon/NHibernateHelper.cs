
using NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

namespace SubServerCommon
{
	public class NHibernateHelper
	{
		public NHibernateHelper()
		{
			InitializeSessionFactory();
		}

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
			_sessionFactory = Fluently.Configure().Database(
			MySQLConfiguration.Standard
				.ConnectionString(cs => cs.Server("localhost.mac")
			                  .Database("darp_db")
			                  .Username("DARP_Server")
			                  .Password("sqlplease")))
				.Mappings(m => m.FluentMappings.AddFromAssemblyOf<NHibernateHelper>())
				.BuildSessionFactory();
		}

		public static ISession OpenSession()
		{
			return SessionFactory.OpenSession();
		}
	}
}

