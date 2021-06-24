using Dapper;
using System.Data;
using System.Data.SQLite;

namespace Manuger.Core.Database
{
	public class DatabaseSchemaUpdater
	{
		public static void Update()
		{
			using (IDbConnection connection = new SQLiteConnection(DatabaseSourceDefinitor.ConnectionString))
			{
				var version = (long)connection.ExecuteScalar("select Version from DbInfo");
			}
		}
	}
}
