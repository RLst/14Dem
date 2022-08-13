using System.IO;
using LeMinhHuy.AI;
using LeMinhHuy.Character;
using UnityEngine;

namespace LeMinhHuy
{
	public partial class GameController : MonoBehaviour
	{
		//Data
		GameData gameData;
		public int teamOneKills;
		public int teamTwoKills;
		public int teamOneDeaths;
		public int teamTwoDeaths;
		public Difficulty teamOneDifficulty;
		public Difficulty teamTwoDifficulty;

		void Awake()
		{
			SetPersistentDataPath();

		}

		void Start()
		{
			//Try loading the config.. if not then just create a new file with default settings
			if (!Load())
			{
				Save(createNew: true);
				Load();
			}
			AppendMatchData();
		}

		void InitTeams()
		{

		}

		void BeginMatch()
		{

		}

		void EndMatch()
		{

		}
	}
}
