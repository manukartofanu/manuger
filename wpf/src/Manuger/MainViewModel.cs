using Manuger.Core;
using System;
using System.ComponentModel;

namespace Manuger
{
	public class MainViewModel : INotifyPropertyChanged
	{
		private Team[] _teams;
		private string _name;

		public Team[] Teams
		{
			get { return _teams; }
			set
			{
				_teams = value;
				RaisePropertyChanged(nameof(Teams));
			}
		}

		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;
				RaisePropertyChanged(nameof(Name));
			}
		}

		#region INotifyPropertyChanged

		[field: NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;

		private void RaisePropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}
}
