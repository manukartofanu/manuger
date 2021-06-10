using Dapper;
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
			foreach (var league in leagues)
			{
				league.Games = GetGamesFinished(league.Id);
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

		public static Tour[] GetTours(long leagueId)
		{
			using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
			{
				var parameter = new { LeagueId = leagueId };
				var output = connection.Query<Tour>("select * from Tour where LeagueId = @LeagueId", parameter);
				return output.ToArray();
			}
		}

		public static void InsertTours(Tour[] tours)
		{
			using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
			{
				for (int i = 0; i < tours.Length; ++i)
				{
					connection.Execute("insert into Tour (LeagueId, Number) values (@LeagueId, @Number)", tours[i]);
				}
			}
		}

		public static Game[] GetGamesInTour(int tourId)
		{
			using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
			{
				var parameter = new { TourId = tourId };
				string query = @"select game.*, hteam.*, ateam.*
												 from Game game
												 join Team hteam on hteam.Id = game.HomeTeamId
												 join Team ateam on ateam.Id = game.AwayTeamId
												 where game.TourId = @TourId";
				var output = connection.Query<Game, Team, Team, Game>(query, (game, homeTeam, awayTeam) =>
				{
					game.HomeTeam = homeTeam;
					game.AwayTeam = awayTeam;
					return game;
				}, parameter);
				return output.ToArray();
			}
		}

		public static Game[] GetGamesFinished(int leagueId)
		{
			using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
			{
				var parameter = new { LeagueId = leagueId };
				string query = @"select game.*, hteam.*, ateam.*
												 from Game game
												 join Team hteam on hteam.Id = game.HomeTeamId
												 join Team ateam on ateam.Id = game.AwayTeamId
												 join Tour tour on tour.Id = game.TourId
												 where game.IsFinished and tour.LeagueId = @LeagueId";
				var output = connection.Query<Game, Team, Team, Game>(query, (game, homeTeam, awayTeam) =>
				{
					game.HomeTeam = homeTeam;
					game.AwayTeam = awayTeam;
					return game;
				}, parameter);
				return output.ToArray();
			}
		}

		public static void InsertGames(Game[] games)
		{
			using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
			{
				for (int i = 0; i < games.Length; ++i)
				{
					connection.Execute(@"insert into Game (TourId, HomeTeamId, AwayTeamId, IsFinished, HomeGoals, AwayGoals)
															 values (@TourId, @HomeTeamId, @AwayTeamId, @IsFinished, @HomeGoals, @AwayGoals)", games[i]);
				}
			}
		}

		public static void UpdateGames(Game[] games)
		{
			using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
			{
				for (int i = 0; i < games.Length; ++i)
				{
					connection.Execute(@"update Game set IsFinished = @IsFinished, HomeGoals = @HomeGoals, AwayGoals =  @AwayGoals
															 where game.Id = @Id", games[i]);
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
