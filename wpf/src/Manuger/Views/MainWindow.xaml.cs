using Manuger.Core;
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
			((MainViewModel)DataContext).Teams = SqliteDataAccess.LoadTeams();
		}

		private void Button_Click_ShowTeams(object sender, RoutedEventArgs e)
		{
			new TeamWindow().ShowDialog();
			((MainViewModel)DataContext).Teams = SqliteDataAccess.LoadTeams();
		}

		private void Button_Click_Schedule(object sender, RoutedEventArgs e)
		{
			IEnumerable<Tour> tours = Schedule.GenerateTours(((MainViewModel)DataContext).Teams, 1);
			SqliteDataAccess.SaveTours(tours.ToArray());
			Tour[] toursWithId = SqliteDataAccess.LoadTours();
			((MainViewModel)DataContext).Tours = toursWithId;
			if (toursWithId.Length > 0)
			{
				((MainViewModel)DataContext).Tour = toursWithId[0];
			}
			IEnumerable<Game> games = Schedule.GenerateSchedule(((MainViewModel)DataContext).Teams, toursWithId);
			SqliteDataAccess.SaveGames(games.ToArray());
			if (toursWithId.Length > 0)
			{
				((MainViewModel)DataContext).GamesInTour = SqliteDataAccess.LoadGamesInTour(toursWithId[0].Id);
			}
		}

		private void Button_Click_Prev(object sender, RoutedEventArgs e)
		{
			var tours = ((MainViewModel)DataContext).Tours;
			var tour = ((MainViewModel)DataContext).Tour;
			int indexOfTour = Array.IndexOf(tours, tour);
			indexOfTour--;
			if (indexOfTour < 0)
			{
				indexOfTour = 0;
			}
			if (tours.Length > 0)
			{
				((MainViewModel)DataContext).GamesInTour = SqliteDataAccess.LoadGamesInTour(tours[indexOfTour].Id);
				((MainViewModel)DataContext).Tour = tours[indexOfTour];
			}
		}

		private void Button_Click_Next(object sender, RoutedEventArgs e)
		{
			var tours = ((MainViewModel)DataContext).Tours;
			var tour = ((MainViewModel)DataContext).Tour;
			int indexOfTour = Array.IndexOf(tours, tour);
			indexOfTour++;
			if (indexOfTour > tours.Length - 1)
			{
				indexOfTour = tours.Length - 1;
			}
			if (tours.Length > 0)
			{
				((MainViewModel)DataContext).GamesInTour = SqliteDataAccess.LoadGamesInTour(tours[indexOfTour].Id);
				((MainViewModel)DataContext).Tour = tours[indexOfTour];
			}
		}
	}
}
