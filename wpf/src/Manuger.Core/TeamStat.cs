
namespace Manuger.Core
{
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
}
