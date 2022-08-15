using System.Collections;
using TMPro;
using UnityEngine;

namespace LeMinhHuy
{
	public class GameUI : MonoBehaviour
	{
		[SerializeField] float UIupdateInterval = 0.1f;	//Reduce CPU
		[Space]
		public TextMeshProUGUI teamOneName;
		public TextMeshProUGUI teamTwoName;
		public TextMeshProUGUI teamOneScore;
		public TextMeshProUGUI teamTwoScore;
		public TextMeshProUGUI roundTime;

		WaitForSeconds updateIntervalWait;
		GameController game;

		void Awake()
		{
			game = FindObjectOfType<GameController>();
			updateIntervalWait = new WaitForSeconds(UIupdateInterval);
		}
		// IEnumerator Start()
		// {
		// 	while (true)
		// 	{
		// 		// if (!game.isMatchInProgress)
		// 		// 	yield break;

		// 		teamOneScore.text = game.teamOneKills.ToString();
		// 		teamTwoScore.text = game.teamTwoKills.ToString();
		// 		roundTime.text = game.matchTimeLeft.ToString();
		// 		yield return updateIntervalWait;
		// 	}
		// }

		void Update()
		{
				teamOneScore.text = game.teamOneKills.ToString();
				teamTwoScore.text = game.teamTwoKills.ToString();
				roundTime.text = Mathf.RoundToInt(game.matchTimeLeft).ToString();
		}
	}
}
