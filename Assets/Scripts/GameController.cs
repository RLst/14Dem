using System;
using System.Collections.Generic;
using System.IO;
using LeMinhHuy.AI;
using LeMinhHuy.Character;
using LeMinhHuy.Controllers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace LeMinhHuy
{
	[RequireComponent(typeof(UltraStateMachine))]
	public partial class GameController : MonoBehaviour
	{
		//Inspector
		[Header("Settings")]
		public Team TeamOne = Team.South;
		public Team TeamTwo = Team.North;
		public int TeamOneNumBots = 5;  //Adjustable in UI
		public int TeamTwoNumBots = 5;  //Adjustable in UI
		public Difficulty TeamOneDifficulty = Difficulty.Passive;   //Adjustable in UI
		public Difficulty TeamTwoDifficulty = Difficulty.Aggressive;    //Adjustable in UI
		public float MatchLength = 7.5f;    //Adjustable in UI

		[Header("Prefabs")]
		public GameObject playerPrefab;
		public GameObject aggressivePrefab;
		public GameObject passiveBotPrefab;

		[Header("UI States")]
		[SerializeField] State pauseState;
		[SerializeField] State gameState;

		[Header("Spawns")]
		[SerializeField] SphereCollider teamOneSpawnArea;
		[SerializeField] SphereCollider teamTwoSpawnArea;

		//Events
		[Space]
		public UnityEvent onWin;
		public UnityEvent onDraw;
		public UnityEvent onLose;
		public UnityEvent onStartGame;
		public UnityEvent onEndGame;

		//Properities
		bool isPaused => usm.currentState == pauseState;
		float matchLengthSeconds => MatchLength * 60f;
		public bool isMatchInProgress { get; private set; } = false;
		public float matchTimeLeft { get; private set; }

		//Data
		GameData gameData;
		[ReadOnlyField] public int teamOneKills;    //TODO: Unity event hookup on each unit
		[ReadOnlyField] public int teamTwoKills;
		[ReadOnlyField] public int teamOneDeaths;
		[ReadOnlyField] public int teamTwoDeaths;


		//Members
		PlayerInputRelay input;
		UltraStateMachine usm;
		List<Unit> teamOneUnits = new List<Unit>();
		List<Unit> teamTwoUnits = new List<Unit>();
		List<Unit> allUnits = new List<Unit>();

		//Callbacks
		public void RegisterKill(Unit unit)
		{
			switch (unit.team)
			{
				case Team.South:
					teamOneKills++;
					break;
				case Team.North:
					teamTwoKills++;
					break;
			}
			CheckMatchEndConditions();
		}

		void CheckMatchEndConditions()
		{
			//TODO: Not fool proof
			if (teamOneKills >= teamTwoUnits.Count)
				EndMatch();
			if (teamTwoKills >= teamOneUnits.Count)
				EndMatch();
		}

		//Main
		void Awake()
		{
			SetPersistentDataPath();

			input = FindObjectOfType<PlayerInputRelay>();
			usm = GetComponent<UltraStateMachine>();
		}

		void Start()
		{
			//Try loading the config..
			if (!Load())
			{
				//if not then just create a new file with inspector params
				gameData = new GameData
				{
					teamOne = TeamOne,
					teamTwo = TeamTwo,
					teamOneNumBots = TeamOneNumBots,
					teamTwoNumBots = TeamTwoNumBots,
					teamOneDifficulty = TeamOneDifficulty,
					teamTwoDifficulty = TeamTwoDifficulty,
					matchLength = MatchLength,
					matchData = new List<MatchData>(),
				};
				Save(gameData);
			}
		}


		public void StartGame()
		{
			//Unstack all states to remove main menu
			usm.ClearStack();
			usm.Stack(gameState);

			BeginMatch();
			onStartGame.Invoke();
		}

		public void RestartMatch()
		{
			//Faulty!

			//Remove all players and begin match again
			foreach (var u in allUnits)
			{
				Destroy(u);
			}
			allUnits.Clear();

			//Go backwards through the array
			// for (int i = allUnits.Count; i >= 0; i--)
			// {
			// 	var u = allUnits[i];
			// 	allUnits.Remove(u);
			// 	Destroy(u.gameObject);
			// }

			BeginMatch();
		}

		public void BeginMatch()
		{
			print("begin match");

			SpawnAndSetPlayer();
			SpawnAndSetAIUnits();
			RegisterAllUnits();

			matchTimeLeft = matchLengthSeconds;
			Time.timeScale = 1f;

			//Begin match
			isMatchInProgress = true;
		}

		private void SpawnAndSetPlayer()
		{
			//Spawn
			var flattenInsideUnitSphere = Random.insideUnitSphere;
			flattenInsideUnitSphere.y = 0;
			var spawnPosition = teamOneSpawnArea.transform.position + flattenInsideUnitSphere * teamOneSpawnArea.radius;
			var player = Instantiate(playerPrefab, spawnPosition, Quaternion.LookRotation(flattenInsideUnitSphere)).GetComponent<Unit>();

			//Set team
			player.team = TeamOne;

			//Register
			teamOneUnits.Add(player);
		}

		private void SpawnAndSetAIUnits()
		{
			//Team one
			for (int i = 0; i < TeamOneNumBots; i++)
			{
				var flattenInsideUnitSphere = Random.insideUnitSphere;
				flattenInsideUnitSphere.y = 0;
				var spawnPosition = teamOneSpawnArea.transform.position + flattenInsideUnitSphere * teamOneSpawnArea.radius;
				var teamOneUnit = Instantiate(TeamOneDifficulty == Difficulty.Aggressive ? aggressivePrefab : passiveBotPrefab, spawnPosition, Quaternion.LookRotation(flattenInsideUnitSphere)).GetComponent<Unit>();
				teamOneUnit.team = TeamOne;
				teamOneUnits.Add(teamOneUnit);
			}

			//Team two
			for (int i = 0; i < TeamTwoNumBots; i++)
			{
				var flattenInsideUnitSphere = Random.insideUnitSphere;
				flattenInsideUnitSphere.y = 0;
				var spawnPosition = teamTwoSpawnArea.transform.position + flattenInsideUnitSphere * teamTwoSpawnArea.radius;
				var teamTwoUnit = Instantiate(TeamTwoDifficulty == Difficulty.Aggressive ? aggressivePrefab : passiveBotPrefab, spawnPosition, Quaternion.LookRotation(flattenInsideUnitSphere)).GetComponent<Unit>();
				teamTwoUnit.team = TeamTwo;
				teamTwoUnits.Add(teamTwoUnit);
			}
		}

		private void RegisterAllUnits()
		{
			allUnits.AddRange(teamOneUnits);
			allUnits.AddRange(teamTwoUnits);
		}

		void Update()
		{
			if (!isMatchInProgress) return;

			//Countdown timer
			matchTimeLeft -= Time.deltaTime;
			if (matchTimeLeft <= 0)
			{
				EndMatch();
			}

			PollPauseButton();
		}


		public void PollPauseButton()
		{
			//Guard; Can not pause in game
			if (!isMatchInProgress) return;

			// if (input.pause)
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				if (!isPaused)
					usm.Stack(pauseState);
				else
					usm.UnStack();
			}
		}

		public void EndMatch()
		{
			isMatchInProgress = false;

			const float slowTime = 0.3f;
			Time.timeScale = slowTime;

			//Display UI
			if (teamOneKills > teamTwoKills)
			{
				onWin.Invoke();
			}
			else if (teamOneKills < teamTwoKills)
			{
				onLose.Invoke();
			}
			else
			{
				onDraw.Invoke();
			}

			//Save match data
			AppendMatchData();
			onEndGame.Invoke();
		}

		public void Restart()
		{
			//Just reset the game for now
			const string MAIN_GAME_SCENE_NAME = "Main";
			SceneManager.LoadScene(MAIN_GAME_SCENE_NAME);
		}

		public void Quit()
		{
			Application.Quit();
		}
	}
}
