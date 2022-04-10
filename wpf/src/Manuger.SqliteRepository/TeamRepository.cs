﻿using Dapper;
using Manuger.Core.Model;
using Manuger.Core.Repository;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace Manuger.SqliteRepository
{
	public class TeamRepository : GenericRepository<Team>, ITeamRepository
	{
		public TeamRepository(string connectionString)
			: base(connectionString, "Team")
		{
		}

		public void CreateItem(Team item)
		{
			using (IDbConnection connection = new SQLiteConnection(_connectionString))
			{
				connection.Execute("insert into Team (Name, CountryId) values (@Name, @CountryId)", item);
			}
		}

		public IEnumerable<Team> GetTeamsByCountry(long countryId)
		{
			using (IDbConnection connection = new SQLiteConnection(_connectionString))
			{
				return connection.Query<Team>($"select * from {_tableName} where CountryId={countryId}");
			}
		}

		public IEnumerable<Team> GetTeamsByLeague(long leagueId)
		{
			using (IDbConnection connection = new SQLiteConnection(_connectionString))
			{
				return connection.Query<Team>($@"
select *
from {_tableName} team
inner join League_Team lteam on team.Id = lteam.TeamId
where lteam.LeagueId={leagueId}");
			}
		}
	}
}
