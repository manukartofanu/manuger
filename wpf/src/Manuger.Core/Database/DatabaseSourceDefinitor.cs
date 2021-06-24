using System;
using System.IO;

namespace Manuger.Core.Database
{
	public class DatabaseSourceDefinitor
	{
		private static readonly string _databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Manuger", "schema.db");

		public static string ConnectionString { get; private set; } = $"Data Source={_databasePath};Version=3;";

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
