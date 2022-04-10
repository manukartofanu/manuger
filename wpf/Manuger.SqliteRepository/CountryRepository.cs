using Dapper;
using Manuger.Model;
using Manuger.Model.Repository.Specific;
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
