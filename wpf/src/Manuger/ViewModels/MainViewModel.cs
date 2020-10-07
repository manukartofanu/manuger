using Manuger.Core;
using System;
using System.ComponentModel;

namespace Manuger.ViewModels
{
	public class MainViewModel : INotifyPropertyChanged
	{
		private Team[] _teams;
		private Tour[] _tours;
		private Tour _tour;
		private Game[] _gamesInTour;
		private TeamStat[] _teamStats;

		public Team[] Teams
		{
			get { return _teams; }
			set
			{
				_teams = value;
				RaisePropertyChanged(nameof(Teams));
			}
		}

		public Tour[] Tours
		{
			get { return _tours; }
			set
			{
				_tours = value;
				RaisePropertyChanged(nameof(Tours));
			}
		}

		public Tour Tour
		{
			get { return _tour; }
			set
			{
				_tour = value;
				RaisePropertyChanged(nameof(Tour));
			}
		}

		public Game[] GamesInTour
		{
			get { return _gamesInTour; }
			set
			{
				_gamesInTour = value;
				RaisePropertyChanged(nameof(GamesInTour));
			}
		}

		public TeamStat[] TeamStats
		{
			get { return _teamStats; }
			set
			{
				_teamStats = value;
				RaisePropertyChanged(nameof(TeamStats));
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
