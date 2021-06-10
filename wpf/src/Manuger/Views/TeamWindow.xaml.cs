using Manuger.Core;
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
			using (var repository = new TeamRepository(SqliteDataAccess.LoadConnectionString()))
			{
				((TeamViewModel)DataContext).Teams = repository.GetAllItems().ToArray();
			}
			using (var repository = new CountryRepository(SqliteDataAccess.LoadConnectionString()))
			{
				((TeamViewModel)DataContext).Countries = repository.GetAllItems().ToArray();
			}
			
		}

		private void Button_Click_AddTeam(object sender, RoutedEventArgs e)
		{
			string name = ((TeamViewModel)DataContext).Name.Trim();
			Country country = ((TeamViewModel)DataContext).Country;
			if (!string.IsNullOrEmpty(name) && country != null)
			{
				using (var repository = new TeamRepository(SqliteDataAccess.LoadConnectionString()))
				{
					repository.CreateItem(new Team { Name = name, CountryId = country.Id });
					((TeamViewModel)DataContext).Teams = repository.GetAllItems().ToArray();
				}
			}
		}
	}
}
