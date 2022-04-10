using Manuger.Core.Model;
using Manuger.Core.Repository.Base;
using System.Collections.Generic;

namespace Manuger.Core.Repository
{
	public interface ITeamRepository : IReadWriteRepository<Team>
	{
		IEnumerable<Team> GetTeamsByCountry(long countryId);
		IEnumerable<Team> GetTeamsByLeague(long leagueId);
	}
}
