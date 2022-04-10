using Dapper;
using Manuger.Model;
using Manuger.Model.Repository.Specific;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace Manuger.Core.Database
{
	public class GameRepository : GenericRepository<Game>, IGameRepository
	{
		public GameRepository(string connectionString)
			: base(connectionString, "Game")
		{
		}

		public void CreateItem(Game item)
		{
			using (IDbConnection connection = new SQLiteConnection(_connectionString))
			{
				connection.Execute(@"insert into Game (TourId, HomeTeamId, AwayTeamId, IsFinished, HomeGoals, AwayGoals)
															values (@TourId, @HomeTeamId, @AwayTeamId, @IsFinished, @HomeGoals, @AwayGoals)", item);
			}
		}

		public Game[] GetGamesInTour(int tourId)
		{
			using (IDbConnection connection = new SQLiteConnection(_connectionString))
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

		public Game[] GetGamesFinished(int leagueId)
		{
			using (IDbConnection connection = new SQLiteConnection(_connectionString))
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

		public void InsertGames(Game[] games)
		{
			using (IDbConnection connection = new SQLiteConnection(_connectionString))
			{
				for (int i = 0; i < games.Length; ++i)
				{
					connection.Execute(@"insert into Game (TourId, HomeTeamId, AwayTeamId, IsFinished, HomeGoals, AwayGoals)
															 values (@TourId, @HomeTeamId, @AwayTeamId, @IsFinished, @HomeGoals, @AwayGoals)", games[i]);
				}
			}
		}

		public void UpdateGames(Game[] games)
		{
			using (IDbConnection connection = new SQLiteConnection(_connectionString))
			{
				for (int i = 0; i < games.Length; ++i)
				{
					connection.Execute(@"update Game set IsFinished = @IsFinished, HomeGoals = @HomeGoals, AwayGoals =  @AwayGoals
															 where game.Id = @Id", games[i]);
				}
			}
		}
	}
}
