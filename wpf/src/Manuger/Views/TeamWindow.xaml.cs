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
	}
}
