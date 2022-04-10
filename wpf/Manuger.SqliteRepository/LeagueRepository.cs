using Dapper;
using Manuger.Core.Model;
using Manuger.Core.Repository;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace Manuger.SqliteRepository
{
	public class LeagueRepository : GenericRepository<League>, ILeagueRepository
	{
		public LeagueRepository(string connectionString)
			: base(connectionString, "League")
		{
		}

		public League[] GetLeagues()
		{
			League[] leagues = new League[0];
			using (IDbConnection connection = new SQLiteConnection(_connectionString))
			{
				leagues = connection.Query<League>("select * from League").ToArray();
				foreach (var league in leagues)
				{
					var parameterLeague = new { LeagueId = league.Id };
					league.Teams = connection.Query<Team>("select * from Team team join League_Team lt on lt.TeamId = team.Id where lt.LeagueId = @LeagueId", parameterLeague).ToArray();
					league.Tours = connection.Query<Tour>("select * from Tour where LeagueId = @LeagueId", parameterLeague).ToArray();
				}
			}
			using (var repository = new GameRepository(DatabaseSourceDefinitor.ConnectionString))
			{
				foreach (var league in leagues)
				{
					league.Games = repository.GetGamesFinished(league.Id);
				}
			}
			return leagues;
		}

		public long InsertLeague(League league)
		{
			long id;
			using (SQLiteConnection connection = new SQLiteConnection(_connectionString))
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

		public void InsertTeamsIntoLeague(long leagueId, Team[] teams)
		{
			using (IDbConnection connection = new SQLiteConnection(_connectionString))
			{
				for (int i = 0; i < teams.Length; ++i)
				{
					var parameter = new { LeagueId = leagueId, TeamId = teams[i].Id };
					connection.Execute("insert into League_Team (LeagueId, TeamId) values (@LeagueId, @TeamId)", parameter);
				}
			}
		}
	}
}
