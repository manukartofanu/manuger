using Manuger.Core;
using Manuger.Core.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manuger.Models
{
	public class LeagueModel
	{
		private readonly IDatabase _repo;
		private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(3, 3);

		public LeagueModel(IDatabase database)
		{
			_repo = database;
		}

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
			return _repo.GetCountryRepository().GetAllItems().ToArray();
		}

		private League CreateLeague(int seasonNumber, Country country)
		{
			var repository = _repo.GetLeagueRepository();
			var league = new League { CountryId = country.Id, Season = seasonNumber };
			league.Id = (int)repository.InsertLeague(league);
			repository.InsertTeamsIntoLeague(league.Id, GetTeamsOfCountry(country.Id));
			return league;
		}

		private Team[] GetTeamsOfCountry(int countryId)
		{
			return _repo.GetTeamRepository().GetTeamsByCountry(countryId).ToArray();
		}

		private Team[] GetTeamsInLeague(int leagueId)
		{
			return _repo.GetTeamRepository().GetTeamsByLeague(leagueId).ToArray();
		}

		private void InsertToursIntoRepository(IEnumerable<Tour> tours)
		{
			_repo.GetTourRepository().InsertTours(tours.ToArray());
		}

		private Tour[] GetToursOfLeague(int leagueId)
		{
			return _repo.GetTourRepository().GetToursInLeague(leagueId);
		}

		private void InsertGamesIntoRepository(IEnumerable<Game> games)
		{
			_repo.GetGameRepository().InsertGames(games.ToArray());
		}
	}
}
