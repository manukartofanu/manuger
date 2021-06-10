using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace Manuger.Core
{
	public class CountryRepository : IRepository<Country>
	{
		private readonly string _connectionString;
		private bool _disposed = false;

		public CountryRepository(string connectionString)
		{
			_connectionString = connectionString;
		}

		public IEnumerable<Country> GetAllItems()
		{
			using (IDbConnection connection = new SQLiteConnection(_connectionString))
			{
				return connection.Query<Country>("select * from Country");
			}
		}

		public Country GetItem(long id)
		{
			using (IDbConnection connection = new SQLiteConnection(_connectionString))
			{
				var parameter = new { CountryId = id };
				return connection.Query<Country>("select * from Country where Id = @CountryId", parameter).AsList().Find(t => t.Id == id);
			}
		}

		public void CreateItem(Country item)
		{
			using (IDbConnection connection = new SQLiteConnection(_connectionString))
			{
				connection.Execute("insert into Country (Id, Code, Name) values (@Id, @Code, @Name)", item);
			}
		}

		public void UpdateItem(Country item)
		{
			using (IDbConnection connection = new SQLiteConnection(_connectionString))
			{
				connection.Execute("update Country set Code=@Code, Name=@Name where Id=@Id", item);
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
