using Manuger.Core;
using System;
using System.ComponentModel;

namespace Manuger.ViewModels
{
	public class MainViewModel : INotifyPropertyChanged
	{
		private League[] _leagues;
		private League _league;
		private Tour _tour;
		private Game[] _gamesInTour;
		private League.TeamStat[] _teamsStat;

		public League[] Leagues
		{
			get { return _leagues; }
			set
			{
				_leagues = value;
				RaisePropertyChanged(nameof(Leagues));
			}
		}

		public League League
		{
			get { return _league; }
			set
			{
				_league = value;
				RaisePropertyChanged(nameof(League));
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

		public League.TeamStat[] TeamsStat
		{
			get { return _teamsStat; }
			set
			{
				_teamsStat = value;
				RaisePropertyChanged(nameof(TeamsStat));
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
