using Manuger.Commands;
using Manuger.Core;
using Manuger.Core.Database;
using Manuger.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace Manuger.ViewModels
{
	public class MainViewModel : INotifyPropertyChanged
	{
		private League[] _leagues;
		private League _league;
		private Tour _tour;
		private Game[] _gamesInTour;
		private League.TeamStat[] _teamsStat;

		public ICommand InitializeCommand { get; private set; }
		public ICommand ShowTeamFillerCommand { get; private set; } = new TeamFillerShowCommand();
		public ICommand GenerateScheduleCommand { get; private set; }
		public ICommand ShowPreviousSeasonCommand { get; private set; }
		public ICommand ShowNextSeasonCommand { get; private set; }
		public ICommand ShowPreviousTourCommand { get; private set; }
		public ICommand ShowNextTourCommand { get; private set; }
		public ICommand GenerateTourResultsCommand { get; private set; }

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

		public MainViewModel()
		{
			InitializeCommand = new RelayCommand((t) => { InitializeData(); });
			GenerateScheduleCommand = new RelayCommand((t) => { GenerateSchedule(); });
			ShowPreviousSeasonCommand = new RelayCommand((t) => { ShowPreviousSeason(); });
			ShowNextSeasonCommand = new RelayCommand((t) => { ShowNextSeason(); });
			ShowPreviousTourCommand = new RelayCommand((t) => { ShowPreviousTour(); });
			ShowNextTourCommand = new RelayCommand((t) => { ShowNextTour(); });
			GenerateTourResultsCommand = new RelayCommand((t) => { GenerateTourResults(); });
		}

		private void InitializeData()
		{
			DatabaseSourceDefinitor.CreateDatabaseIfNotExist();
			DatabaseSchemaUpdater.Update();
			using (var repository = new LeagueRepository(DatabaseSourceDefinitor.ConnectionString))
			{
				Leagues = repository.GetLeagues();
			}
			if (Leagues.Length > 0)
			{
				int lastSeason = Leagues.Max(t => t.Season);
				League = Leagues.First(t => t.Season == lastSeason);
				Tour = League.Tours[0];
				using (var repository = new GameRepository(DatabaseSourceDefinitor.ConnectionString))
				{
					GamesInTour = repository.GetGamesInTour(Tour.Id);
				}
				League.Calculate();
				TeamsStat = League.TeamStats;
			}
		}

		private void GenerateSchedule()
		{
			int lastSeasonNumber = Leagues.Length == 0 ? 0 : Leagues.Max(t => t.Season);
			new LeagueModel().GenerateSeason(lastSeasonNumber + 1);
			using (var repository = new LeagueRepository(DatabaseSourceDefinitor.ConnectionString))
			{
				Leagues = repository.GetLeagues();
			}
			if (Leagues.Length > 0)
			{
				int lastSeason = Leagues.Max(t => t.Season);
				League = Leagues.First(t => t.Season == lastSeason);
				Tour = League.Tours[0];
				using (var repository = new GameRepository(DatabaseSourceDefinitor.ConnectionString))
				{
					GamesInTour = repository.GetGamesInTour(Tour.Id);
				}
				League.Calculate();
				TeamsStat = League.TeamStats;
			}
		}

		private void SafeDecrement(ref int index, int lowerBound)
		{
			index--;
			if (index < lowerBound)
			{
				index = lowerBound;
			}
		}

		private void SafeIncrement(ref int index, int upperBound)
		{
			index++;
			if (index > upperBound)
			{
				index = upperBound;
			}
		}

		private void ShowPreviousSeason()
		{
			int indexOfLeague = Array.IndexOf(Leagues, League);
			SafeDecrement(ref indexOfLeague, 0);
			if (Leagues.Length > 0)
			{
				League = Leagues[indexOfLeague];
				Tour = League.Tours[0];
				using (var repository = new GameRepository(DatabaseSourceDefinitor.ConnectionString))
				{
					GamesInTour = repository.GetGamesInTour(Tour.Id);
				}
				League.Calculate();
				TeamsStat = League.TeamStats;
			}
		}

		private void ShowNextSeason()
		{
			int indexOfLeague = Array.IndexOf(Leagues, League);
			SafeIncrement(ref indexOfLeague, Leagues.Length - 1);
			if (Leagues.Length > 0)
			{
				League = Leagues[indexOfLeague];
				Tour = League.Tours[0];
				using (var repository = new GameRepository(DatabaseSourceDefinitor.ConnectionString))
				{
					GamesInTour = repository.GetGamesInTour(Tour.Id);
				}
				League.Calculate();
				TeamsStat = League.TeamStats;
			}
		}

		private void ShowPreviousTour()
		{
			var tours = League.Tours;
			int indexOfTour = Array.IndexOf(tours, Tour);
			SafeDecrement(ref indexOfTour, 0);
			if (tours.Length > 0)
			{
				using (var repository = new GameRepository(DatabaseSourceDefinitor.ConnectionString))
				{
					GamesInTour = repository.GetGamesInTour(tours[indexOfTour].Id);
				}
				Tour = tours[indexOfTour];
			}
		}

		private void ShowNextTour()
		{
			var tours = League.Tours;
			int indexOfTour = Array.IndexOf(tours, Tour);
			SafeIncrement(ref indexOfTour, tours.Length - 1);
			if (tours.Length > 0)
			{
				using (var repository = new GameRepository(DatabaseSourceDefinitor.ConnectionString))
				{
					GamesInTour = repository.GetGamesInTour(tours[indexOfTour].Id);
				}
				Tour = tours[indexOfTour];
			}
		}

		private void GenerateTourResults()
		{
			int? leagueId = League?.Id;
			if (Tour != null)
			{
				GamesInTour.GenerateResults();
				using (var repository = new GameRepository(DatabaseSourceDefinitor.ConnectionString))
				{
					repository.UpdateGames(GamesInTour);
				}
				using (var repository = new LeagueRepository(DatabaseSourceDefinitor.ConnectionString))
				{
					Leagues = repository.GetLeagues();
				}
				League = Leagues.FirstOrDefault(t => t.Id == leagueId);
				Tour = League.Tours.FirstOrDefault(t => t.Id == Tour.Id);
				using (var repository = new GameRepository(DatabaseSourceDefinitor.ConnectionString))
				{
					GamesInTour = repository.GetGamesInTour(Tour.Id);
				}
				League.Calculate();
				TeamsStat = League.TeamStats;
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
