using Dapper;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace Manuger.Core
{
	public class SqliteDataAccess
	{
		public static Team[] LoadTeams()
		{
			using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
			{
				var output = connection.Query<Team>("select * from Team", new DynamicParameters());
				return output.ToArray();
			}
		}

		public static void SaveTeam(Team team)
		{
			using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
			{
				connection.Execute("insert into Team (Name) values (@Name)", team);
			}
		}

		public static Tour[] LoadTours()
		{
			using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
			{
				var output = connection.Query<Tour>("select * from Tour", new DynamicParameters());
				return output.ToArray();
			}
		}

		public static void SaveTours(Tour[] tours)
		{
			using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
			{
				for (int i = 0; i < tours.Length; ++i)
				{
					connection.Execute("insert into Tour (Season, Number) values (@Season, @Number)", tours[i]);
				}
			}
		}

		public static GameEx[] LoadGamesInTour(int tourId)
		{
			using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
			{
				var parameter = new { TourId = tourId };
				string query = @"select game.*, hteam.*, ateam.*
												 from Game game
												 join Team hteam on hteam.Id = game.HomeTeamId
												 join Team ateam on ateam.Id = game.AwayTeamId
												 where game.TourId = @TourId";
				var output = connection.Query<GameEx, Team, Team, GameEx>(query, (game, homeTeam, awayTeam) =>
				{
					game.HomeTeam = homeTeam;
					game.AwayTeam = awayTeam;
					return game;
				}, parameter);
				return output.ToArray();
			}
		}

		public static void SaveGames(Game[] games)
		{
			using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
			{
				for (int i = 0; i < games.Length; ++i)
				{
					connection.Execute("insert into Game (TourId, HomeTeamId, AwayTeamId, IsFinished) values (@TourId, @HomeTeamId, @AwayTeamId, @IsFinished)", games[i]);
				}
			}
		}

		private static string LoadConnectionString(string id = "Default")
		{
			return ConfigurationManager.ConnectionStrings[id].ConnectionString;
		}
	}
}
