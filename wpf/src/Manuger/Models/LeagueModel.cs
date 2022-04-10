using Manuger.Model;
using Manuger.Model.Repository.Specific;
using Manuger.SqliteRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manuger.Models
{
	public class LeagueModel
	{
		private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(3, 3);

		public void GenerateSeason(int newSeasonNumber)
		{
			var newLeagues = CreateLeaguesOfSeason(newSeasonNumber);
			GenerateSchedulesOfSeason(newLeagues);
		}

		private List<League> CreateLeaguesOfSeason(int seasonNumber)
		{
			Country[] countries = GetAllCountries();
			List<League> newLeagues = new List<League>();
			foreach (var country in countries)
			{
				newLeagues.Add(CreateLeague(seasonNumber, country));
			}
			return newLeagues;
		}

		private void GenerateSchedulesOfSeason(List<League> leagues)
		{
			Task[] tasks = new Task[leagues.Count];
			for (int i = 0; i < leagues.Count; ++i)
			{
				League league = leagues[i];
				tasks[i] = Task.Run(() =>
				{
					_semaphore.Wait();
					try
					{
						GenerateScheduleForLeague(league);
					}
					finally
					{
						_semaphore.Release();
					}
				});
			}
			Task.WaitAll(tasks);
		}

		private void GenerateScheduleForLeague(League league)
		{
			Team[] teamsInLeague = GetTeamsInLeague(league.Id);
			IEnumerable<Tour> tours = Schedule.GenerateTours(teamsInLeague, league.Id);
			InsertToursIntoRepository(tours);
			Tour[] toursWithId = GetToursOfLeague(league.Id);
			IEnumerable<Game> games = Schedule.GenerateSchedule(teamsInLeague, toursWithId);
			InsertGamesIntoRepository(games);
		}

		private Country[] GetAllCountries()
		{
			using (ICountryRepository repository = new CountryRepository(DatabaseSourceDefinitor.ConnectionString))
			{
				return repository.GetAllItems().ToArray();
			}
		}

		private League CreateLeague(int seasonNumber, Country country)
		{
			using (ILeagueRepository repository = new LeagueRepository(DatabaseSourceDefinitor.ConnectionString))
			{
				var league = new League { CountryId = country.Id, Season = seasonNumber };
				league.Id = (int)repository.InsertLeague(league);
				repository.InsertTeamsIntoLeague(league.Id, GetTeamsOfCountry(country.Id));
				return league;
			}
		}

		private Team[] GetTeamsOfCountry(int countryId)
		{
			using (ITeamRepository repository = new TeamRepository(DatabaseSourceDefinitor.ConnectionString))
			{
				return repository.GetTeamsByCountry(countryId).ToArray();
			}
		}

		private Team[] GetTeamsInLeague(int leagueId)
		{
			using (ITeamRepository repository = new TeamRepository(DatabaseSourceDefinitor.ConnectionString))
			{
				return repository.GetTeamsByLeague(leagueId).ToArray();
			}
		}

		private void InsertToursIntoRepository(IEnumerable<Tour> tours)
		{
			using (ITourRepository repository = new TourRepository(DatabaseSourceDefinitor.ConnectionString))
			{
				repository.InsertTours(tours.ToArray());
			}
		}

		private Tour[] GetToursOfLeague(int leagueId)
		{
			using (ITourRepository repository = new TourRepository(DatabaseSourceDefinitor.ConnectionString))
			{
				return repository.GetToursInLeague(leagueId);
			}
		}

		private void InsertGamesIntoRepository(IEnumerable<Game> games)
		{
			using (IGameRepository repository = new GameRepository(DatabaseSourceDefinitor.ConnectionString))
			{
				repository.InsertGames(games.ToArray());
			}
		}
	}
}
