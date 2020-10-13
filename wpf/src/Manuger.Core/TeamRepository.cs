using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace Manuger.Core
{
	public class TeamRepository : IRepository<Team>
	{
		private readonly string _connectionString;
		private bool _disposed = false;

		public TeamRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public IEnumerable<Team> GetAllItems()
		{
			using (IDbConnection connection = new SQLiteConnection(_connectionString))
			{
				return connection.Query<Team>("select * from Team");
			}
		}

		public Team GetItem(long id)
		{
			using (IDbConnection connection = new SQLiteConnection(_connectionString))
			{
				var parameter = new { TeamId = id };
				return connection.Query<Team>("select * from Team where Id = @TeamId", parameter).AsList().Find(t => t.Id == id);
			}
		}

		public void CreateItem(Team item)
		{
			using (IDbConnection connection = new SQLiteConnection(_connectionString))
			{
				connection.Execute("insert into Team (Name, CountryId) values (@Name, @CountryId)", item);
			}
		}

		public void UpdateItem(Team item)
		{
			using (IDbConnection connection = new SQLiteConnection(_connectionString))
			{
				connection.Execute("update Team set Name=@Name, CountryId=@CountryId where Id=@Id", item);
			}
		}

		public virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					//dispose smth
				}
			}
			_disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
