using System.Linq;

namespace Manuger.Core
{
	public static class League
	{
		public static TeamStat[] Calculate(Team[] teams, Game[] games)
		{
			var result = teams.Select(team => new TeamStat { TeamId = team.Id, Name = team.Name }).ToArray();
			foreach (var game in games.Where(t => t.IsFinished))
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
			return result.OrderByDescending(t => t.Points).OrderByDescending(t => t.GoalDifference).ToArray();
		}
	}
}
