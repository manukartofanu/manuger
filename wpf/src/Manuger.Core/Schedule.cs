using System;
using System.Collections.Generic;
using System.Linq;

namespace Manuger.Core
{
	public static class Schedule
	{
		private static Random _scoreRandom = new Random();

		public static IEnumerable<Tour> GenerateTours(Team[] teams, int season)
		{
			int count = teams.Length * 2 - 2;
			for (int i = 0; i < count; ++i)
			{
				yield return new Tour { Season = season, Number = i + 1 };
			}
		}

		public static IEnumerable<Game> GenerateSchedule(Team[] teams, Tour[] tours)
		{
			int toursCount = tours.Length / 2;
			int gamesCountInTour = teams.Length / 2;
			//tour 1
			for (int i = 0; i < gamesCountInTour; ++i)
			{
				yield return GetFirstRoundGame(teams[i].Id, teams[teams.Length - i - 1].Id, tours.First(t => t.Number == 1).Id);
			}
			//tour 2 ...
			for (int i = 1; i < toursCount; ++i)
			{
				int tourId = tours.First(t => t.Number == i + 1).Id;
				int gamesCountInFirstDiagonal = (i + 1) / 2;
				for (int j = 0; j < gamesCountInFirstDiagonal; ++j)
				{
					yield return GetFirstRoundGame(teams[j].Id, teams[i - j].Id, tourId);
				}
				int gamesCountInSecondDiagonal = gamesCountInTour - gamesCountInFirstDiagonal - 1;
				for (int j = 0; j < gamesCountInSecondDiagonal; ++j)
				{
					yield return GetFirstRoundGame(teams[i + j + 1].Id, teams[teams.Length - 2 - j].Id, tourId);
				}
				if (i % 2 == 1)
				{
					yield return GetFirstRoundGame(teams[i + 1 + gamesCountInSecondDiagonal].Id, teams[teams.Length - 1].Id, tourId);
				}
				else
				{
					yield return GetFirstRoundGame(teams[gamesCountInFirstDiagonal].Id, teams[teams.Length - 1].Id, tourId);
				}
			}
			//tour 1 + toursCount
			for (int i = 0; i < gamesCountInTour; ++i)
			{
				yield return GetSecondRoundGame(teams[i].Id, teams[teams.Length - i - 1].Id, tours.First(t => t.Number == 1 + toursCount).Id);
			}
			//tour 2 + toursCount ...
			for (int i = 1; i < toursCount; ++i)
			{
				int tourId = tours.First(t => t.Number == i + 1 + toursCount).Id;
				int gamesCountInFirstDiagonal = (i + 1) / 2;
				for (int j = 0; j < gamesCountInFirstDiagonal; ++j)
				{
					yield return GetSecondRoundGame(teams[j].Id, teams[i - j].Id, tourId);
				}
				int gamesCountInSecondDiagonal = gamesCountInTour - gamesCountInFirstDiagonal - 1;
				for (int j = 0; j < gamesCountInSecondDiagonal; ++j)
				{
					yield return GetSecondRoundGame(teams[i + j + 1].Id, teams[teams.Length - 2 - j].Id, tourId);
				}
				if (i % 2 == 1)
				{
					yield return GetSecondRoundGame(teams[i + 1 + gamesCountInSecondDiagonal].Id, teams[teams.Length - 1].Id, tourId);
				}
				else
				{
					yield return GetSecondRoundGame(teams[gamesCountInFirstDiagonal].Id, teams[teams.Length - 1].Id, tourId);
				}
			}
		}

		private static Game GetFirstRoundGame(int teamId1, int teamId2, int tourId)
		{
			int lowerId = teamId1 > teamId2 ? teamId2 : teamId1;
			int upperId = teamId1 > teamId2 ? teamId1 : teamId2;
			int homeId = (teamId1 + teamId2) % 2 == 1 ? lowerId : upperId;
			int awayId = homeId == lowerId ? upperId : lowerId;
			return new Game
			{
				HomeTeamId = homeId,
				AwayTeamId = awayId,
				TourId = tourId
			};
		}

		private static Game GetSecondRoundGame(int teamId1, int teamId2, int tourId)
		{
			int lowerId = teamId1 > teamId2 ? teamId2 : teamId1;
			int upperId = teamId1 > teamId2 ? teamId1 : teamId2;
			int homeId = (teamId1 + teamId2) % 2 == 1 ? upperId : lowerId;
			int awayId = homeId == lowerId ? upperId : lowerId;
			return new Game
			{
				HomeTeamId = homeId,
				AwayTeamId = awayId,
				TourId = tourId
			};
		}

		public static void GenerateResults(this Game[] games)
		{
			foreach (var game in games)
			{
				game.HomeGoals = _scoreRandom.Next(4);
				game.AwayGoals = _scoreRandom.Next(4);
				game.IsFinished = true;
			}
		}
	}
}