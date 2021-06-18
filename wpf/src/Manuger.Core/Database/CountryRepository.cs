using Dapper;
using System.Data;
using System.Data.SQLite;

namespace Manuger.Core.Database
{
	public class CountryRepository : GenericRepository<Country>, IRepository<Country>
	{
		public CountryRepository(string connectionString)
			: base(connectionString, "Country")
		{
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
	}
}
