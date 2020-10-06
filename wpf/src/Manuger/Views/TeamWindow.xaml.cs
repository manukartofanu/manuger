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
			((TeamViewModel)DataContext).Teams = SqliteDataAccess.LoadTeams();
		}

		private void Button_Click_AddTeam(object sender, RoutedEventArgs e)
		{
			string name = ((TeamViewModel)DataContext).Name;
			if (!string.IsNullOrEmpty(name))
			{
				SqliteDataAccess.SaveTeam(new Team { Name = name });
			}
			((TeamViewModel)DataContext).Teams = SqliteDataAccess.LoadTeams();
		}
	}
}
