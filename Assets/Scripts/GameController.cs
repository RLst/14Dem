using System;
using System.IO;
using LeMinhHuy.AI;
using LeMinhHuy.Character;
using LeMinhHuy.Controllers;
using UnityEngine;

namespace LeMinhHuy
{
	[RequireComponent(typeof(UltraStateMachine))]
	public partial class GameController : MonoBehaviour
	{
		//Inspector
		[SerializeField] State pauseState;
		[SerializeField] State gameState;
		[Space]
		[SerializeField] Collider teamOneSpawnArea;
		[SerializeField] Collider teamTwoSpawnArea;

		//Properities
		bool isPaused => usm.currentState == pauseState;

		//Data
		GameData gameData;
		[ReadOnlyField] public int teamOneKills;
		[ReadOnlyField] public int teamTwoKills;
		[ReadOnlyField] public int teamOneDeaths;
		[ReadOnlyField] public int teamTwoDeaths;
		[ReadOnlyField] public Difficulty teamOneDifficulty;
		[ReadOnlyField] public Difficulty teamTwoDifficulty;

		//Members
		PlayerInputRelay input;
		UltraStateMachine usm;
		bool isMatchBegan = false;

		void Awake()
		{
			SetPersistentDataPath();

			input = FindObjectOfType<PlayerInputRelay>();
			usm = GetComponent<UltraStateMachine>();
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

		public void BeginMatch()
		{
			print("begin match");
			//Unstack all states to remove main menu
			usm.ClearStack();
			usm.Stack(gameState);

			//Remove all units

			//Instantiate and place units randomly based on their team areas
			//For each team
			//Instantiate

			//Begin match
			isMatchBegan = true;
		}

		void Update()
		{
			if (!isMatchBegan) return;

			PollPauseButton();
		}

		public void PollPauseButton()
		{
			//Guard; Can not pause in game
			if (!isMatchBegan) return;

			if (Input.GetKeyDown(KeyCode.Escape))

			// if (input.pause)
			{
				if (!isPaused)
					usm.Stack(pauseState);
				else
					usm.UnStack();
			}
		}

		public void EndMatch()
		{
			isMatchBegan = false;

		}

		public void RestartMatch()
		{
			throw new NotImplementedException();
		}

		public void Quit()
		{
			Application.Quit();
		}
	}
}
