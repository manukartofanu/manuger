using Manuger.Core;
using Manuger.Core.Repository;

namespace Manuger.SqliteRepository
{
	public class SqliteDatabase : IDatabase
	{
		private readonly SqliteDataSource _dataSource;
		private ICountryRepository _countryRepository;
		private IGameRepository _gameRepository;
		private ILeagueRepository _leagueRepository;
		private ITeamRepository _teamRepository;
		private ITourRepository _tourRepository;

		public SqliteDatabase()
		{
			_dataSource = new SqliteDataSource();
			CreateRepositories();
		}

		private void CreateRepositories()
		{
			string connectionString = _dataSource.GetSource();
			_countryRepository = new CountryRepository(connectionString);
			_gameRepository = new GameRepository(connectionString);
			_leagueRepository = new LeagueRepository(connectionString);
			_teamRepository = new TeamRepository(connectionString);
			_tourRepository = new TourRepository(connectionString);
		}

		public void SetDataSource(string path)
		{
			_dataSource.SetSource(path);
			CreateRepositories();
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
