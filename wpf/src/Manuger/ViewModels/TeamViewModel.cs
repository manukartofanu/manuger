using Manuger.Core;
using System;
using System.ComponentModel;

namespace Manuger.ViewModels
{
	class TeamViewModel : INotifyPropertyChanged
	{
		private Team[] _teams;
		private Country[] _countries;
		private string _name;
		private Country _country;

		public Team[] Teams
		{
			get { return _teams; }
			set
			{
				_teams = value;
				RaisePropertyChanged(nameof(Teams));
			}
		}

		public Country[] Countries
		{
			get { return _countries; }
			set
			{
				_countries = value;
				RaisePropertyChanged(nameof(Countries));
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

		public Country Country
		{
			get { return _country; }
			set
			{
				_country = value;
				RaisePropertyChanged(nameof(Country));
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
