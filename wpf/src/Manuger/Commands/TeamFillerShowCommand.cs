using Manuger.Core;
using Manuger.Views;
using System;
using System.Windows.Input;

namespace Manuger.Commands
{
	public class TeamFillerShowCommand : ICommand
	{
		private readonly IRepository _repo;

		public TeamFillerShowCommand(IRepository repo)
		{
			_repo = repo;
		}

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
			new TeamWindow(_repo).ShowDialog();
		}
	}
}
