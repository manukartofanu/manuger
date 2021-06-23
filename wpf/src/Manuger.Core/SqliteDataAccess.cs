using Dapper;
using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace Manuger.Core
{
	public static class SqliteDataAccess
	{
		private static readonly string _databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Manuger", "schema.db");

		public static void UpdateDatabaseSchema()
		{
			using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
			{
				var version = (long)connection.ExecuteScalar("select Version from DbInfo");
			}
		}

		public static string LoadConnectionString()
		{
			return $"Data Source={_databasePath};Version=3;";
		}

		public static void CreateDatabaseIfNotExist()
		{
			if (!File.Exists(_databasePath))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(_databasePath));
				File.Copy(@".\schema.db", _databasePath);
			}
		}
	}
}
