using Dapper;
using System.Data;
using System.Data.SQLite;

namespace Manuger.Core.Database
{
	public class DatabaseSchemaUpdater
	{
		public static void Update()
		{
			long version = 0;
			using (IDbConnection connection = new SQLiteConnection(DatabaseSourceDefinitor.ConnectionString))
			{
				version = (long)connection.ExecuteScalar("select Version from DbInfo");
			}
			if (version < 1)
			{
				Update001();
			}
		}

		private static void Update001()
		{
			using (var repository = new CountryRepository(DatabaseSourceDefinitor.ConnectionString))
			{
				repository.CreateItem(new Country { Id = 250, Code = "FRA", Name = "France" });
			}
			using (IDbConnection connection = new SQLiteConnection(DatabaseSourceDefinitor.ConnectionString))
			{
				connection.Execute("update	DbInfo set Version = 1");
			}
		}
	}
}
