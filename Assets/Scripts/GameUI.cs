using System.Collections;
using TMPro;
using UnityEngine;

namespace LeMinhHuy
{
	public class GameUI : MonoBehaviour
	{
		public TextMeshProUGUI teamOneScore;
		public TextMeshProUGUI teamTwoScore;
		GameController game;
		float UIupdateInterval = 0.1f;
		WaitForSeconds updateIntervalWait;
		void Awake()
		{
			game = FindObjectOfType<GameController>();
			updateIntervalWait = new WaitForSeconds(UIupdateInterval);
		}
		IEnumerator Start()
		{
			while (true)
			{
				teamOneScore.text = game.teamOneKills.ToString();
				teamTwoScore.text = game.teamTwoKills.ToString();
				yield return updateIntervalWait;
			}
		}
	}
}
