using Manuger.Core.Repository;

namespace Manuger.Core
{
	public interface IDatabase
	{
		void SetDataSource(string path);
		ICountryRepository GetCountryRepository();
		IGameRepository GetGameRepository();
		ILeagueRepository GetLeagueRepository();
		ITeamRepository GetTeamRepository();
		ITourRepository GetTourRepository();
	}
}
