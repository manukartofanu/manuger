using Manuger.Core;
using Manuger.Core.Database;
using Manuger.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Manuger.Views
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			DatabaseSourceDefinitor.CreateDatabaseIfNotExist();
			DatabaseSchemaUpdater.Update();
			using (var repository = new LeagueRepository(DatabaseSourceDefinitor.ConnectionString))
			{
				((MainViewModel)DataContext).Leagues = repository.GetLeagues();
			}
			var leagues = ((MainViewModel)DataContext).Leagues;
			if (leagues.Length > 0)
			{
				int lastSeason = leagues.Max(t => t.Season);
				((MainViewModel)DataContext).League = leagues.First(t => t.Season == lastSeason);
				((MainViewModel)DataContext).Tour = ((MainViewModel)DataContext).League.Tours[0];
				using (var repository = new GameRepository(DatabaseSourceDefinitor.ConnectionString))
				{
					((MainViewModel)DataContext).GamesInTour = repository.GetGamesInTour(((MainViewModel)DataContext).Tour.Id);
				}
				((MainViewModel)DataContext).League.Calculate();
				((MainViewModel)DataContext).TeamsStat = ((MainViewModel)DataContext).League.TeamStats;
			}
		}

		private void Button_Click_ShowTeams(object sender, RoutedEventArgs e)
		{
			new TeamWindow().ShowDialog();
		}

		private void Button_Click_Schedule(object sender, RoutedEventArgs e)
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
			var leagues = ((MainViewModel)DataContext).Leagues;
			int lastSeasonNumber = leagues.Length == 0 ? 0 : leagues.Max(t => t.Season);
			List<League> newLeagues = new List<League>();
			using (var repository = new LeagueRepository(DatabaseSourceDefinitor.ConnectionString))
			{
				int newSeasonNumber = lastSeasonNumber + 1;
				foreach (var country in countries)
				{
					var league = new League { CountryId = country.Id, Season = newSeasonNumber };
					league.Id = (int)repository.InsertLeague(league);
					newLeagues.Add(league);
					repository.InsertTeamsIntoLeague(league.Id, teams.Where(t => t.CountryId == country.Id).ToArray());
				}
			}
			foreach (var league in newLeagues)
			{
				Team[] teamsInLeague = teams.Where(t => t.CountryId == league.CountryId).ToArray();
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
			using (var repository = new LeagueRepository(DatabaseSourceDefinitor.ConnectionString))
			{
				((MainViewModel)DataContext).Leagues = repository.GetLeagues();
			}
			if (leagues.Length > 0)
			{
				int lastSeason = leagues.Max(t => t.Season);
				((MainViewModel)DataContext).League = leagues.First(t => t.Season == lastSeason);
				((MainViewModel)DataContext).Tour = ((MainViewModel)DataContext).League.Tours[0];
				using (var repository = new GameRepository(DatabaseSourceDefinitor.ConnectionString))
				{
					((MainViewModel)DataContext).GamesInTour = repository.GetGamesInTour(((MainViewModel)DataContext).Tour.Id);
				}
				((MainViewModel)DataContext).League.Calculate();
				((MainViewModel)DataContext).TeamsStat = ((MainViewModel)DataContext).League.TeamStats;
			}
		}

		private void Button_Click_Season_Prev(object sender, RoutedEventArgs e)
		{
			var leagues = ((MainViewModel)DataContext).Leagues;
			var league = ((MainViewModel)DataContext).League;
			int indexOfLeague = Array.IndexOf(leagues, league);
			indexOfLeague--;
			if (indexOfLeague < 0)
			{
				indexOfLeague = 0;
			}
			if (leagues.Length > 0)
			{
				((MainViewModel)DataContext).League = leagues[indexOfLeague];
				((MainViewModel)DataContext).Tour = ((MainViewModel)DataContext).League.Tours[0];
				using (var repository = new GameRepository(DatabaseSourceDefinitor.ConnectionString))
				{
					((MainViewModel)DataContext).GamesInTour = repository.GetGamesInTour(((MainViewModel)DataContext).Tour.Id);
				}
				((MainViewModel)DataContext).League.Calculate();
				((MainViewModel)DataContext).TeamsStat = ((MainViewModel)DataContext).League.TeamStats;
			}
		}

		private void Button_Click_Season_Next(object sender, RoutedEventArgs e)
		{
			var leagues = ((MainViewModel)DataContext).Leagues;
			var league = ((MainViewModel)DataContext).League;
			int indexOfLeague = Array.IndexOf(leagues, league);
			indexOfLeague++;
			if (indexOfLeague > leagues.Length - 1)
			{
				indexOfLeague = leagues.Length - 1;
			}
			if (leagues.Length > 0)
			{
				((MainViewModel)DataContext).League = leagues[indexOfLeague];
				((MainViewModel)DataContext).Tour = ((MainViewModel)DataContext).League.Tours[0];
				using (var repository = new GameRepository(DatabaseSourceDefinitor.ConnectionString))
				{
					((MainViewModel)DataContext).GamesInTour = repository.GetGamesInTour(((MainViewModel)DataContext).Tour.Id);
				}
				((MainViewModel)DataContext).League.Calculate();
				((MainViewModel)DataContext).TeamsStat = ((MainViewModel)DataContext).League.TeamStats;
			}
		}

		private void Button_Click_Prev(object sender, RoutedEventArgs e)
		{
			var tours = ((MainViewModel)DataContext).League.Tours;
			var tour = ((MainViewModel)DataContext).Tour;
			int indexOfTour = Array.IndexOf(tours, tour);
			indexOfTour--;
			if (indexOfTour < 0)
			{
				indexOfTour = 0;
			}
			if (tours.Length > 0)
			{
				using (var repository = new GameRepository(DatabaseSourceDefinitor.ConnectionString))
				{
					((MainViewModel)DataContext).GamesInTour = repository.GetGamesInTour(tours[indexOfTour].Id);
				}
				((MainViewModel)DataContext).Tour = tours[indexOfTour];
			}
		}

		private void Button_Click_Next(object sender, RoutedEventArgs e)
		{
			var tours = ((MainViewModel)DataContext).League.Tours;
			var tour = ((MainViewModel)DataContext).Tour;
			int indexOfTour = Array.IndexOf(tours, tour);
			indexOfTour++;
			if (indexOfTour > tours.Length - 1)
			{
				indexOfTour = tours.Length - 1;
			}
			if (tours.Length > 0)
			{
				using (var repository = new GameRepository(DatabaseSourceDefinitor.ConnectionString))
				{
					((MainViewModel)DataContext).GamesInTour = repository.GetGamesInTour(tours[indexOfTour].Id);
				}
				((MainViewModel)DataContext).Tour = tours[indexOfTour];
			}
		}

		private void Button_Click_GenerateResults(object sender, RoutedEventArgs e)
		{
			int? leagueId = ((MainViewModel)DataContext).League?.Id;
			var games = ((MainViewModel)DataContext).GamesInTour;
			var tour = ((MainViewModel)DataContext).Tour;
			if (tour != null)
			{
				games.GenerateResults();
				using (var repository = new GameRepository(DatabaseSourceDefinitor.ConnectionString))
				{
					repository.UpdateGames(games);
				}
				using (var repository = new LeagueRepository(DatabaseSourceDefinitor.ConnectionString))
				{
					((MainViewModel)DataContext).Leagues = repository.GetLeagues();
				}
				((MainViewModel)DataContext).League = ((MainViewModel)DataContext).Leagues.FirstOrDefault(t => t.Id == leagueId);
				((MainViewModel)DataContext).Tour = ((MainViewModel)DataContext).League.Tours.FirstOrDefault(t => t.Id == tour.Id);
				using (var repository = new GameRepository(DatabaseSourceDefinitor.ConnectionString))
				{
					((MainViewModel)DataContext).GamesInTour = repository.GetGamesInTour(tour.Id);
				}
				((MainViewModel)DataContext).League.Calculate();
				((MainViewModel)DataContext).TeamsStat = ((MainViewModel)DataContext).League.TeamStats;
			}
		}
	}
}
