using Dapper;
using System.Data;
using System.Data.SQLite;

namespace Manuger.Core.Database
{
	public class TeamRepository : GenericRepository<Team>, IRepository<Team>
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
	}
}
