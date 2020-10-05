using System.Collections.Generic;
using System.Linq;

namespace Manuger.Core
{
	public static class Schedule
	{
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
			//1 6 | 2 5 | 3 4
			//1 2 | 3 5 | 4 6
			//1 3 | 2 6 | 4 5
			//1 4 | 2 3 | 5 6
			//1 5 | 2 4 | 3 6
			int toursCount = tours.Length / 2;
			int gamesCountInTour = teams.Length / 2;
			//tour 1
			for (int i = 0; i < gamesCountInTour; ++i)
			{
				yield return new Game
				{
					HomeTeamId = teams[i].Id,
					AwayTeamId = teams[teams.Length - i - 1].Id,
					TourId = tours.First(t => t.Number == 1).Id
				};
			}
			//tour 2 ...
			for (int i = 1; i < toursCount; ++i)
			{
				int tourId = tours.First(t => t.Number == i + 1).Id;
				int gamesCountInFirstDiagonal = (i + 1) / 2;
				for (int j = 0; j < gamesCountInFirstDiagonal; ++j)
				{
					yield return new Game
					{
						HomeTeamId = teams[j].Id,
						AwayTeamId = teams[i - j].Id,
						TourId = tourId
					};
				}
				int gamesCountInSecondDiagonal = gamesCountInTour - gamesCountInFirstDiagonal - 1;
				for (int j = 0; j < gamesCountInSecondDiagonal; ++j)
				{
					yield return new Game
					{
						HomeTeamId = teams[i + j + 1].Id,
						AwayTeamId = teams[teams.Length - 2 - j].Id,
						TourId = tourId
					};
				}
				if (i % 2 == 1)
				{
					yield return new Game
					{
						HomeTeamId = teams[i + 1 + gamesCountInSecondDiagonal].Id,
						AwayTeamId = teams[teams.Length - 1].Id,
						TourId = tourId
					};
				}
				else
				{
					yield return new Game
					{
						HomeTeamId = teams[gamesCountInFirstDiagonal].Id,
						AwayTeamId = teams[teams.Length - 1].Id,
						TourId = tourId
					};
				}
			}
		}
	}
}