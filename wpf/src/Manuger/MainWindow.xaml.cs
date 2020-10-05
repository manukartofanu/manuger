using Manuger.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Manuger
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

		private void Button_Click_AddTeam(object sender, RoutedEventArgs e)
		{
			string name = ((MainViewModel)DataContext).Name;
			if (!string.IsNullOrEmpty(name))
			{
				SqliteDataAccess.SaveTeam(new Team { Name = name });
			}
			((MainViewModel)DataContext).Teams = SqliteDataAccess.LoadTeams();
		}

		private void Button_Click_Schedule(object sender, RoutedEventArgs e)
		{
			IEnumerable<Tour> tours = Schedule.GenerateTours(((MainViewModel)DataContext).Teams, 1);
			SqliteDataAccess.SaveTours(tours.ToArray());
			Tour[] toursWithId = SqliteDataAccess.LoadTours();
			IEnumerable<Game> games = Schedule.GenerateSchedule(((MainViewModel)DataContext).Teams, toursWithId);
			SqliteDataAccess.SaveGames(games.ToArray());
		}
	}
}
