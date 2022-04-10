using Dapper;
using Manuger.Core;
using Manuger.Core.Repository.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace Manuger.SqliteRepository
{
	public class GenericRepository<TItem> : IReadRepository<TItem>
		where TItem : class, IIdentable
	{
		protected readonly string _connectionString;
		protected readonly string _tableName;
		protected bool _disposed = false;

		public GenericRepository(string connectionString, string tableName)
		{
			_connectionString = connectionString;
			_tableName = tableName;
		}

		public TItem GetItem(long id)
		{
			using (IDbConnection connection = new SQLiteConnection(_connectionString))
			{
				var parameter = new { Id = id };
				return connection.Query<TItem>($"select * from {_tableName} where Id = @Id", parameter).AsList().Find(t => t.Id == id);
			}
		}

		public IEnumerable<TItem> GetAllItems()
		{
			using (IDbConnection connection = new SQLiteConnection(_connectionString))
			{
				return connection.Query<TItem>($"select * from {_tableName}");
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
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
	}
}
