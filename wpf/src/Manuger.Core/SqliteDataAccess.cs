using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace Manuger.Core
{
	public class SqliteDataAccess
	{
		public static List<Team> LoadTeams()
		{
			using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
			{
				var output = connection.Query<Team>("select * from Team", new DynamicParameters());
				return output.ToList();
			}
		}

		public static void SaveTeam(Team team)
		{
			using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
			{
				connection.Execute("insert into Person (Name) values (@Name)", team);
			}
		}

		private static string LoadConnectionString(string id = "Default")
		{
			return ConfigurationManager.ConnectionStrings[id].ConnectionString;
		}
	}
}
