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
		public TeamWindow(IRepository repo)
		{
			InitializeComponent();
			((TeamViewModel)DataContext).Repo = repo;
		}
	}
}
