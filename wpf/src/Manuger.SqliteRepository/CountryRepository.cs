using Dapper;
using Manuger.Core.Model;
using Manuger.Core.Repository;
using System.Data;
using System.Data.SQLite;

namespace Manuger.SqliteRepository
{
	public class CountryRepository : GenericRepository<Country>, ICountryRepository
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
	}
}
