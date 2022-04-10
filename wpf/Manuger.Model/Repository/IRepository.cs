using Manuger.Model.Repository.Specific;

namespace Manuger.Model.Repository
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
