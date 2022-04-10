using Manuger.Commands;
using Manuger.Core.Model;
using Manuger.SqliteRepository;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace Manuger.ViewModels
{
	class TeamViewModel : INotifyPropertyChanged
	{
		private Team[] _teams;
		private Country[] _countries;
		private string _name;
		private Country _country;

		public ICommand LoadInitialDataCommand { get; private set; }
		public ICommand SelectCountryCommand { get; private set; }
		public ICommand AddTeamCommand { get; private set; }

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

		public TeamViewModel()
		{
			LoadInitialDataCommand = new RelayCommand((t) => { LoadInitialData(); });
			SelectCountryCommand = new RelayCommand((t) => { RefreshTeams(); }, (t) => { return Country != null; });
			AddTeamCommand = new RelayCommand((t) => { AddTeam(); }, (t) => { return Country != null && !string.IsNullOrEmpty(Name); });
		}

		private void LoadInitialData()
		{
			using (var repository = new CountryRepository(DatabaseSourceDefinitor.ConnectionString))
			{
				Countries = repository.GetAllItems().ToArray();
			}
		}

		private void RefreshTeams()
		{
			using (var repository = new TeamRepository(DatabaseSourceDefinitor.ConnectionString))
			{
				Teams = repository.GetTeamsByCountry(Country.Id).ToArray();
			}
		}

		private void AddTeam()
		{
			string name = Name.Trim();
			if (!string.IsNullOrEmpty(name) && Country != null)
			{
				using (var repository = new TeamRepository(DatabaseSourceDefinitor.ConnectionString))
				{
					repository.CreateItem(new Team { Name = name, CountryId = Country.Id });
					Teams = repository.GetTeamsByCountry(Country.Id).ToArray();
				}
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
