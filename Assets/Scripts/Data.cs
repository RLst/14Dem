using System;
using System.Collections.Generic;
using LeMinhHuy.AI;
using LeMinhHuy.Character;

namespace LeMinhHuy
{
	public class GameData
	{
		public Team teamOne = Team.South;
		public Team teamTwo = Team.North;

		public int teamOneNumBots = 4;
		public int teamTwoNumBots = 5;

		public Difficulty teamOneDifficulty = Difficulty.Aggressive;
		public Difficulty teamTwoDifficulty = Difficulty.Passive;

		public float matchLength = 5f;

		public List<MatchData> matchData = new List<MatchData>();   //NOTE: Data is actually plural
	}

	//Battle logs
	public class MatchData
	{
		public DateTime date;
		public int teamOneKills;
		public int teamTwoKills;
		public int teamOneDeaths;
		public int teamTwoDeaths;
		public Difficulty teamOneDifficulty;
		public Difficulty teamTwoDifficulty;
	}
}
