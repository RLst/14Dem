using System;
using System.IO;
using LeMinhHuy.AI;
using LeMinhHuy.Character;
using UnityEngine;

namespace LeMinhHuy
{
	public partial class GameController : MonoBehaviour
	{
		string PERSISTENT_DATA_PATH;
		const string SETTINGS_FILE = "/settings.cfg";

		void SetPersistentDataPath() => PERSISTENT_DATA_PATH = Application.dataPath;

		//Save the current game data ie. use after UI adjust
		public void Save()
		{
			Save(gameData);
		}

		public void Save(GameData gameData)
		{
			try
			{
				var savedJson = JsonUtility.ToJson(gameData, prettyPrint: true);
				File.WriteAllText(PERSISTENT_DATA_PATH + SETTINGS_FILE, savedJson);
			}
			catch (Exception e)
			{
				Debug.LogWarning(e, this);
			}
		}

		public bool Load()
		{
			if (File.Exists(PERSISTENT_DATA_PATH + SETTINGS_FILE))
			{
				var loadedJson = File.ReadAllText(PERSISTENT_DATA_PATH + SETTINGS_FILE);
				gameData = JsonUtility.FromJson<GameData>(loadedJson);
				return true;
			}
			return false;
		}

		public void AppendMatchData(bool alsoSave = true, MatchData customData = null)
		{
			if (customData != null)
			{
				gameData.matchData.Add(customData);
			}
			else
			{
				gameData.matchData.Add(new MatchData
				{
					date = System.DateTime.Now,
					teamOneKills = teamOneKills,
					teamTwoKills = teamTwoKills,
					teamOneDeaths = teamOneDeaths,
					teamTwoDeaths = teamTwoDeaths,
					teamOneDifficulty = TeamOneDifficulty,
					teamTwoDifficulty = TeamTwoDifficulty,
				});
			}

			if (alsoSave) Save();
		}
	}
}
