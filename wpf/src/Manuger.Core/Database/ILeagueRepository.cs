
namespace Manuger.Core.Database
{
	public interface ILeagueRepository : IReadRepository<League>
	{
		League[] GetLeagues();
		long InsertLeague(League league);
		void InsertTeamsIntoLeague(long leagueId, Team[] teams);
	}
}
