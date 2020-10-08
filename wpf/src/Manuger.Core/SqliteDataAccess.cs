﻿using Dapper;
using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace Manuger.Core
{
	public static class SqliteDataAccess
	{
		private static readonly string _databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Manuger", "schema.db");

		public static void UpdateDatabaseSchema()
		{
			using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
			{
				var version = (long)connection.ExecuteScalar("select Version from DbInfo");
				if (version != 1)
				{
					connection.Query(File.ReadAllText(@"DbScripts\1.sql"));
				}
			}
		}

		public static Team[] GetTeams()
		{
			using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
			{
				var output = connection.Query<Team>("select * from Team", new DynamicParameters());
				return output.ToArray();
			}
		}

		public static void InsertTeam(Team team)
		{
			using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
			{
				connection.Execute("insert into Team (Name) values (@Name)", team);
			}
		}

		public static Tour[] GetTours()
		{
			using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
			{
				var output = connection.Query<Tour>("select * from Tour", new DynamicParameters());
				return output.ToArray();
			}
		}

		public static void InsertTours(Tour[] tours)
		{
			using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
			{
				for (int i = 0; i < tours.Length; ++i)
				{
					connection.Execute("insert into Tour (Season, Number) values (@Season, @Number)", tours[i]);
				}
			}
		}

		public static Game[] GetGamesInTour(int tourId)
		{
			using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
			{
				var parameter = new { TourId = tourId };
				string query = @"select game.*, hteam.*, ateam.*
												 from Game game
												 join Team hteam on hteam.Id = game.HomeTeamId
												 join Team ateam on ateam.Id = game.AwayTeamId
												 where game.TourId = @TourId";
				var output = connection.Query<Game, Team, Team, Game>(query, (game, homeTeam, awayTeam) =>
				{
					game.HomeTeam = homeTeam;
					game.AwayTeam = awayTeam;
					return game;
				}, parameter);
				return output.ToArray();
			}
		}

		public static Game[] GetGamesFinished()
		{
			using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
			{
				string query = @"select game.*, hteam.*, ateam.*
												 from Game game
												 join Team hteam on hteam.Id = game.HomeTeamId
												 join Team ateam on ateam.Id = game.AwayTeamId
												 where game.IsFinished";
				var output = connection.Query<Game, Team, Team, Game>(query, (game, homeTeam, awayTeam) =>
				{
					game.HomeTeam = homeTeam;
					game.AwayTeam = awayTeam;
					return game;
				});
				return output.ToArray();
			}
		}

		public static void InsertGames(Game[] games)
		{
			using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
			{
				for (int i = 0; i < games.Length; ++i)
				{
					connection.Execute(@"insert into Game (TourId, HomeTeamId, AwayTeamId, IsFinished, HomeGoals, AwayGoals)
															 values (@TourId, @HomeTeamId, @AwayTeamId, @IsFinished, @HomeGoals, @AwayGoals)", games[i]);
				}
			}
		}

		public static void UpdateGames(Game[] games)
		{
			using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
			{
				for (int i = 0; i < games.Length; ++i)
				{
					connection.Execute(@"update Game set IsFinished = @IsFinished, HomeGoals = @HomeGoals, AwayGoals =  @AwayGoals
															 where game.Id = @Id", games[i]);
				}
			}
		}

		private static string LoadConnectionString()
		{
			return $"Data Source={_databasePath};Version=3;";
		}

		public static void CreateDatabaseIfNotExist()
		{
			if (!File.Exists(_databasePath))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(_databasePath));
				File.Copy(@".\schema.db", _databasePath);
			}
		}
	}
}
