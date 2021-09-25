using Manuger.Views;
using System;
using System.Windows.Input;

namespace Manuger.Commands
{
	public class TeamFillerShowCommand : ICommand
	{
		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			new TeamWindow().ShowDialog();
		}
	}
}
