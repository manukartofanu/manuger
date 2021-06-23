using Dapper;
using Manuger.Core.Database;
using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;

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

		public static League[] GetLeagues()
		{
			League[] leagues = new League[0];
			using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
			{
				leagues = connection.Query<League>("select * from League").ToArray();
				foreach (var league in leagues)
				{
					var parameterLeague = new { LeagueId = league.Id };
					league.Teams = connection.Query<Team>("select * from Team team join League_Team lt on lt.TeamId = team.Id where lt.LeagueId = @LeagueId", parameterLeague).ToArray();
					league.Tours = connection.Query<Tour>("select * from Tour where LeagueId = @LeagueId", parameterLeague).ToArray();
				}
			}
			using (var repository = new GameRepository(SqliteDataAccess.LoadConnectionString()))
			{
				foreach (var league in leagues)
				{
					league.Games = repository.GetGamesFinished(league.Id);
				}
			}
			return leagues;
		}

		public static long InsertLeague(League league)
		{
			long id;
			using (SQLiteConnection connection = new SQLiteConnection(LoadConnectionString()))
			{
				connection.Open();
				SQLiteTransaction transaction = null;
				transaction = connection.BeginTransaction();
				connection.Execute("insert into League (CountryId, Season) values (@CountryId, @Season)", league);
				id = connection.LastInsertRowId;
				transaction.Commit();
				connection.Close();
			}
			return id;
		}

		public static void InsertTeamsIntoLeague(long leagueId, Team[] teams)
		{
			using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
			{
				for (int i = 0; i < teams.Length; ++i)
				{
					var parameter = new { LeagueId = leagueId, TeamId = teams[i].Id };
					connection.Execute("insert into League_Team (LeagueId, TeamId) values (@LeagueId, @TeamId)", parameter);
				}
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
