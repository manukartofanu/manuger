using Manuger.Core;
using Manuger.Core.Database;
using Manuger.ViewModels;
using System.Linq;
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
			using (var repository = new CountryRepository(DatabaseSourceDefinitor.ConnectionString))
			{
				((TeamViewModel)DataContext).Countries = repository.GetAllItems().ToArray();
			}
		}

		private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			using (var repository = new TeamRepository(DatabaseSourceDefinitor.ConnectionString))
			{
				((TeamViewModel)DataContext).Teams = repository.GetTeamsByCountry(((TeamViewModel)DataContext).Country.Id).ToArray();
			}
		}

		private void Button_Click_AddTeam(object sender, RoutedEventArgs e)
		{
			string name = ((TeamViewModel)DataContext).Name.Trim();
			Country country = ((TeamViewModel)DataContext).Country;
			if (!string.IsNullOrEmpty(name) && country != null)
			{
				using (var repository = new TeamRepository(DatabaseSourceDefinitor.ConnectionString))
				{
					repository.CreateItem(new Team { Name = name, CountryId = country.Id });
					((TeamViewModel)DataContext).Teams = repository.GetTeamsByCountry(((TeamViewModel)DataContext).Country.Id).ToArray();
				}
			}
		}
	}
}
