﻿using Dapper;
using Manuger.Core.Model;
using System.Data;
using System.Data.SQLite;

namespace Manuger.SqliteRepository
{
	internal class SqliteSchemaUpdater
	{
		private readonly string _connectionString;

		public SqliteSchemaUpdater(string connectionString)
		{
			_connectionString = connectionString;
		}

		public void Update()
		{
			long version = 0;
			using (IDbConnection connection = new SQLiteConnection(_connectionString))
			{
				version = (long)connection.ExecuteScalar("select Version from DbInfo");
			}
			if (version < 1)
			{
				Update001();
			}
			if (version < 2)
			{
				Update002();
			}
		}

		private void Update001()
		{
			using (var repository = new CountryRepository(_connectionString))
			{
				repository.CreateItem(new Country { Id = 250, Code = "FRA", Name = "France" });
			}
			using (IDbConnection connection = new SQLiteConnection(_connectionString))
			{
				connection.Execute("update	DbInfo set Version = 1");
			}
		}

		private void Update002()
		{
			using (var repository = new CountryRepository(_connectionString))
			{
				repository.CreateItem(new Country { Id = 4401, Code = "ENG", Name = "England" });
				repository.CreateItem(new Country { Id = 49, Code = "GER", Name = "Germany" });
				repository.CreateItem(new Country { Id = 39, Code = "ITA", Name = "Italy" });
			}
			using (IDbConnection connection = new SQLiteConnection(_connectionString))
			{
				connection.Execute("update	DbInfo set Version = 2");
			}
		}
	}
}
