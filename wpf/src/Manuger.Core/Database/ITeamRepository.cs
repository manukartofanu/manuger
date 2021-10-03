using System.Collections.Generic;

namespace Manuger.Core.Database
{
	public interface ITeamRepository : IRepository<Team>
	{
		IEnumerable<Team> GetTeamsByCountry(long countryId);
		IEnumerable<Team> GetTeamsByLeague(long leagueId);
	}
}
