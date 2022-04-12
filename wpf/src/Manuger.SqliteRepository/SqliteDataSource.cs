using System;
using System.IO;

namespace Manuger.SqliteRepository
{
	internal class SqliteDataSource
	{
		private static readonly string _defaultDatabasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Manuger", "schema.db");
		private static readonly string _connectionStringTemplate = "Data Source={0};Version=3;";

		private string DatabasePath { get; set; }

		public SqliteDataSource()
		{
			DatabasePath = _defaultDatabasePath;
		}

		public string GetSource()
		{
			if (!File.Exists(DatabasePath))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(DatabasePath));
				File.Copy(@".\schema.db", DatabasePath);
			}
			else
			{
				var updater = new SqliteSchemaUpdater(string.Format(_connectionStringTemplate, DatabasePath));
				updater.Update();
			}
			return string.Format(_connectionStringTemplate, DatabasePath);
		}

		public string SetSource(string path)
		{
			DatabasePath = path;
			return GetSource();
		}
	}
}
