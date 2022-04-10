
namespace Manuger.Model.Repository.Specific
{
	public interface ILeagueRepository : IReadRepository<League>
	{
		League[] GetLeagues();
		long InsertLeague(League league);
		void InsertTeamsIntoLeague(long leagueId, Team[] teams);
	}
}
