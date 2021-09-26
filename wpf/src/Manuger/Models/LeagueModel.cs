﻿using Manuger.Core;
using Manuger.Core.Database;
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
			var newLeagues = GenerateNewLeagues(newSeasonNumber);
			Task[] tasks = new Task[newLeagues.Count];
			for (int i = 0; i < newLeagues.Count; ++i)
			{
				League league = newLeagues[i];
				tasks[i] = Task.Run(() =>
				{
					_semaphore.Wait();
					try
					{
						GenerateSchedule(league);
					}
					finally
					{
						_semaphore.Release();
					}
				});
			}
			Task.WaitAll(tasks);
		}

		private List<League> GenerateNewLeagues(int newSeasonNumber)
		{
			Country[] countries;
			using (var repository = new CountryRepository(DatabaseSourceDefinitor.ConnectionString))
			{
				countries = repository.GetAllItems().ToArray();
			}
			Team[] teams;
			using (var repository = new TeamRepository(DatabaseSourceDefinitor.ConnectionString))
			{
				teams = repository.GetAllItems().ToArray();
			}
			List<League> newLeagues = new List<League>();
			using (var repository = new LeagueRepository(DatabaseSourceDefinitor.ConnectionString))
			{
				foreach (var country in countries)
				{
					var league = new League { CountryId = country.Id, Season = newSeasonNumber };
					league.Id = (int)repository.InsertLeague(league);
					newLeagues.Add(league);
					repository.InsertTeamsIntoLeague(league.Id, teams.Where(t => t.CountryId == country.Id).ToArray());
				}
			}
			return newLeagues;
		}

		private void GenerateSchedule(League league)
		{
			Team[] teamsInLeague;
			using (var repository = new TeamRepository(DatabaseSourceDefinitor.ConnectionString))
			{
				teamsInLeague = repository.GetTeamsByLeague(league.Id).ToArray();
			}
			IEnumerable<Tour> tours = Schedule.GenerateTours(teamsInLeague, league.Id);
			using (var repository = new TourRepository(DatabaseSourceDefinitor.ConnectionString))
			{
				repository.InsertTours(tours.ToArray());
			}
			Tour[] toursWithId;
			using (var repository = new TourRepository(DatabaseSourceDefinitor.ConnectionString))
			{
				toursWithId = repository.GetToursInLeague(league.Id);
			}
			IEnumerable<Game> games = Schedule.GenerateSchedule(teamsInLeague, toursWithId);
			using (var repository = new GameRepository(DatabaseSourceDefinitor.ConnectionString))
			{
				repository.InsertGames(games.ToArray());
			}
		}
	}
}
