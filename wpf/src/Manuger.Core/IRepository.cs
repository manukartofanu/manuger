using Manuger.Core.Repository;

namespace Manuger.Core
{
	public interface IRepository
	{
		ICountryRepository GetCountryRepository();
		IGameRepository GetGameRepository();
		ILeagueRepository GetLeagueRepository();
		ITeamRepository GetTeamRepository();
		ITourRepository GetTourRepository();
	}
}
