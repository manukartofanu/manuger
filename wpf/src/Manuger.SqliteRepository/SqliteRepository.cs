using Manuger.Core;
using Manuger.Core.Repository;

namespace Manuger.SqliteRepository
{
	public class SqliteRepository : IRepository
	{
		private readonly ICountryRepository _countryRepository;
		private readonly IGameRepository _gameRepository;
		private readonly ILeagueRepository _leagueRepository;
		private readonly ITeamRepository _teamRepository;
		private readonly ITourRepository _tourRepository;

		public SqliteRepository()
		{
			_countryRepository = new CountryRepository(DatabaseSourceDefinitor.ConnectionString);
			_gameRepository = new GameRepository(DatabaseSourceDefinitor.ConnectionString);
			_leagueRepository = new LeagueRepository(DatabaseSourceDefinitor.ConnectionString);
			_teamRepository = new TeamRepository(DatabaseSourceDefinitor.ConnectionString);
			_tourRepository = new TourRepository(DatabaseSourceDefinitor.ConnectionString);
		}

		public ICountryRepository GetCountryRepository()
		{
			return _countryRepository;
		}

		public IGameRepository GetGameRepository()
		{
			return _gameRepository;
		}

		public ILeagueRepository GetLeagueRepository()
		{
			return _leagueRepository;
		}

		public ITeamRepository GetTeamRepository()
		{
			return _teamRepository;
		}

		public ITourRepository GetTourRepository()
		{
			return _tourRepository;
		}
	}
}
