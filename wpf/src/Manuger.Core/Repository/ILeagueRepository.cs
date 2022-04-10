using Manuger.Core.Model;
using Manuger.Core.Repository.Base;

namespace Manuger.Core.Repository
{
	public interface ILeagueRepository : IReadRepository<League>
	{
		League[] GetLeagues();
		long InsertLeague(League league);
		void InsertTeamsIntoLeague(long leagueId, Team[] teams);
	}
}
