using Manuger.Commands;
using Manuger.Core;
using Manuger.Core.Model;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace Manuger.ViewModels
{
	class TeamViewModel : INotifyPropertyChanged
	{
		internal IRepository Repo { get; set; }

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
			Countries = Repo.GetCountryRepository().GetAllItems().ToArray();
		}

		private void RefreshTeams()
		{
			Teams = Repo.GetTeamRepository().GetTeamsByCountry(Country.Id).ToArray();
		}

		private void AddTeam()
		{
			string name = Name.Trim();
			if (!string.IsNullOrEmpty(name) && Country != null)
			{
				var repo = Repo.GetTeamRepository();
				repo.CreateItem(new Team { Name = name, CountryId = Country.Id });
				Teams = repo.GetTeamsByCountry(Country.Id).ToArray();
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
