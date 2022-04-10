using System.Collections.Generic;

namespace Manuger.Model.Repository.Specific
{
	public interface ITeamRepository : IReadWriteRepository<Team>
	{
		IEnumerable<Team> GetTeamsByCountry(long countryId);
		IEnumerable<Team> GetTeamsByLeague(long leagueId);
	}
}
