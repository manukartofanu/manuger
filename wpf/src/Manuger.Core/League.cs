using System.Linq;

namespace Manuger.Core
{
	public class League
	{
		public int Id { get; set; }
		public int CountryId { get; set; }
		public Country Country { get; set; }
		public int Season { get; set; }
		public Team[] Teams { get; set; }
		public Tour[] Tours { get; set; }
		public Game[] Games { get; set; }
		public TeamStat[] TeamStats { get; set; }

		public class TeamStat
		{
			public int TeamId { get; set; }
			public string Name { get; set; }
			public int Points { get; set; }
			public int Wins { get; set; }
			public int Ties { get; set; }
			public int Loses { get; set; }
			public int GoalsScored { get; set; }
			public int GoalsConceded { get; set; }
			public int GoalDifference { get { return GoalsScored - GoalsConceded; } }
		}

		public void Calculate()
		{
			var result = Teams.Select(team => new TeamStat { TeamId = team.Id, Name = team.Name }).ToArray();
			foreach (var game in Games.Where(t => t.IsFinished))
			{
				var homeTeam = result.First(t => t.TeamId == game.HomeTeamId);
				var awayTeam = result.First(t => t.TeamId == game.AwayTeamId);
				int homeGoals = game.HomeGoals ?? 0;
				int awayGoals = game.AwayGoals ?? 0;
				homeTeam.GoalsScored += homeGoals;
				homeTeam.GoalsConceded += awayGoals;
				awayTeam.GoalsScored += awayGoals;
				awayTeam.GoalsConceded += homeGoals;
				if (homeGoals == awayGoals)
				{
					homeTeam.Ties++;
					awayTeam.Ties++;
				}
				else if (homeGoals > awayGoals)
				{
					homeTeam.Wins++;
					awayTeam.Loses++;
				}
				else
				{
					awayTeam.Wins++;
					homeTeam.Loses++;
				}
			}
			for (int i = 0; i < result.Length; ++i)
			{
				result[i].Points = result[i].Wins * 3 + result[i].Ties;
			}
			TeamStats = result.OrderByDescending(t => t.Points).OrderByDescending(t => t.GoalDifference).ToArray();
		}
	}
}
