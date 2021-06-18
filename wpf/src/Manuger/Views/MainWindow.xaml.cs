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
			SqliteDataAccess.CreateDatabaseIfNotExist();
			SqliteDataAccess.UpdateDatabaseSchema();
			((MainViewModel)DataContext).Leagues = SqliteDataAccess.GetLeagues();
			var leagues = ((MainViewModel)DataContext).Leagues;
			if (leagues.Length > 0)
			{
				int lastSeason = leagues.Max(t => t.Season);
				((MainViewModel)DataContext).League = leagues.First(t => t.Season == lastSeason);
				((MainViewModel)DataContext).Tour = ((MainViewModel)DataContext).League.Tours[0];
				((MainViewModel)DataContext).GamesInTour = SqliteDataAccess.GetGamesInTour(((MainViewModel)DataContext).Tour.Id);
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
			using (var repository = new CountryRepository(SqliteDataAccess.LoadConnectionString()))
			{
				countries = repository.GetAllItems().ToArray();
			}
			Team[] teams;
			using (var repository = new TeamRepository(SqliteDataAccess.LoadConnectionString()))
			{
				teams = repository.GetAllItems().ToArray();
			}
			var leagues = ((MainViewModel)DataContext).Leagues;
			int lastSeasonNumber = leagues.Length == 0 ? 0 : leagues.Max(t => t.Season);
			long leagueId = SqliteDataAccess.InsertLeague(new League { CountryId = countries[0].Id, Season = lastSeasonNumber + 1 });
			SqliteDataAccess.InsertTeamsIntoLeague(leagueId, teams);
			IEnumerable<Tour> tours = Schedule.GenerateTours(teams, leagueId);
			using (var repository = new TourRepository(SqliteDataAccess.LoadConnectionString()))
			{
				repository.InsertTours(tours.ToArray());
			}
			Tour[] toursWithId;
			using (var repository = new TourRepository(SqliteDataAccess.LoadConnectionString()))
			{
				toursWithId = repository.GetToursInLeague(leagueId);
			}
			IEnumerable<Game> games = Schedule.GenerateSchedule(teams, toursWithId);
			SqliteDataAccess.InsertGames(games.ToArray());
			((MainViewModel)DataContext).Leagues = SqliteDataAccess.GetLeagues();
			if (leagues.Length > 0)
			{
				int lastSeason = leagues.Max(t => t.Season);
				((MainViewModel)DataContext).League = leagues.First(t => t.Season == lastSeason);
				((MainViewModel)DataContext).Tour = ((MainViewModel)DataContext).League.Tours[0];
				((MainViewModel)DataContext).GamesInTour = SqliteDataAccess.GetGamesInTour(((MainViewModel)DataContext).Tour.Id);
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
				((MainViewModel)DataContext).GamesInTour = SqliteDataAccess.GetGamesInTour(((MainViewModel)DataContext).Tour.Id);
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
				((MainViewModel)DataContext).GamesInTour = SqliteDataAccess.GetGamesInTour(((MainViewModel)DataContext).Tour.Id);
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
				((MainViewModel)DataContext).GamesInTour = SqliteDataAccess.GetGamesInTour(tours[indexOfTour].Id);
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
				((MainViewModel)DataContext).GamesInTour = SqliteDataAccess.GetGamesInTour(tours[indexOfTour].Id);
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
				SqliteDataAccess.UpdateGames(games);
				((MainViewModel)DataContext).Leagues = SqliteDataAccess.GetLeagues();
				((MainViewModel)DataContext).League = ((MainViewModel)DataContext).Leagues.FirstOrDefault(t => t.Id == leagueId);
				((MainViewModel)DataContext).Tour = ((MainViewModel)DataContext).League.Tours.FirstOrDefault(t => t.Id == tour.Id);
				((MainViewModel)DataContext).GamesInTour = SqliteDataAccess.GetGamesInTour(tour.Id);
				((MainViewModel)DataContext).League.Calculate();
				((MainViewModel)DataContext).TeamsStat = ((MainViewModel)DataContext).League.TeamStats;
			}
		}
	}
}
