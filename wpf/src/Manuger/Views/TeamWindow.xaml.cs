using Manuger.Core;
using Manuger.ViewModels;
using System.Windows;

namespace Manuger.Views
{
	/// <summary>
	/// Interaction logic for TeamWindow.xaml
	/// </summary>
	public partial class TeamWindow : Window
	{
		public TeamWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			((TeamViewModel)DataContext).Teams = SqliteDataAccess.GetTeams();
			((TeamViewModel)DataContext).Countries = SqliteDataAccess.GetCountries();
		}

		private void Button_Click_AddTeam(object sender, RoutedEventArgs e)
		{
			string name = ((TeamViewModel)DataContext).Name.Trim();
			Country country = ((TeamViewModel)DataContext).Country;
			if (!string.IsNullOrEmpty(name) && country != null)
			{
				SqliteDataAccess.InsertTeam(new Team { Name = name, CountryId = country.Id });
			}
			((TeamViewModel)DataContext).Teams = SqliteDataAccess.GetTeams();
		}
	}
}
