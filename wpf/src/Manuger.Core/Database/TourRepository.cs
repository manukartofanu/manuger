﻿using Dapper;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace Manuger.Core.Database
{
	public class TourRepository : GenericRepository<Tour>, ITourRepository
	{
		public TourRepository(string connectionString)
			: base(connectionString, "Tour")
		{
		}

		public void CreateItem(Tour item)
		{
			using (IDbConnection connection = new SQLiteConnection(_connectionString))
			{
				connection.Execute("insert into Tour (Id, LeagueId, Number) values (@Id, @LeagueId, @Number)", item);
			}
		}

		public void UpdateItem(Tour item)
		{
			using (IDbConnection connection = new SQLiteConnection(_connectionString))
			{
				connection.Execute("update Tour set LeagueId=@LeagueId, Number=@Number where Id=@Id", item);
			}
		}

		public Tour[] GetToursInLeague(long leagueId)
		{
			using (IDbConnection connection = new SQLiteConnection(_connectionString))
			{
				var parameter = new { LeagueId = leagueId };
				var output = connection.Query<Tour>("select * from Tour where LeagueId = @LeagueId", parameter);
				return output.ToArray();
			}
		}

		public void InsertTours(Tour[] tours)
		{
			using (IDbConnection connection = new SQLiteConnection(_connectionString))
			{
				for (int i = 0; i < tours.Length; ++i)
				{
					connection.Execute("insert into Tour (LeagueId, Number) values (@LeagueId, @Number)", tours[i]);
				}
			}
		}
	}
}
