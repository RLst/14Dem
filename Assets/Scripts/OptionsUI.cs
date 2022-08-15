using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LeMinhHuy
{
	public class OptionsUI : MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI teamOneBotsText;
		[SerializeField] Slider teamOneBotSlider;
		[SerializeField] TMP_Dropdown teamOneDiffDropdown;

		[SerializeField] TextMeshProUGUI teamTwoBotsText;
		[SerializeField] Slider teamTwoBotSlider;
		[SerializeField] TMP_Dropdown teamTwoDiffDropdown;

		[SerializeField] TextMeshProUGUI matchDurationText;
		[SerializeField] Slider matchDurationSlider;

		GameController game;

		void Awake()
		{
			game = FindObjectOfType<GameController>();
		}

		void Start()
		{
			//Initialize values of options
			SetT1BotText(game.TeamOneNumBots);
			teamOneBotSlider.SetValueWithoutNotify(game.TeamOneNumBots);
			teamOneDiffDropdown.SetValueWithoutNotify((int)game.TeamOneDifficulty);

			SetT2BotText(game.TeamTwoNumBots);
			teamTwoBotSlider.SetValueWithoutNotify(game.TeamTwoNumBots);
			teamTwoDiffDropdown.SetValueWithoutNotify((int)game.TeamTwoDifficulty);

			SetMatchDurationText(game.MatchLength);
			matchDurationSlider.SetValueWithoutNotify(game.MatchLength);
		}

		public void OnChangeT1Bots(float input)
		{
			game.TeamOneNumBots = Mathf.RoundToInt(input);
			SetT1BotText(input);
		}
		private void SetT1BotText(float input)
		{
			teamOneBotsText.text = "Bots: " + input;
		}

		public void OnChangeT1Difficulty(int input)
		{
			game.TeamOneDifficulty = (AI.Difficulty)input;
		}

		public void OnChangeT2Bots(float input)
		{
			game.TeamTwoNumBots = Mathf.RoundToInt(input);
			SetT2BotText(input);
		}
		private void SetT2BotText(float input)
		{
			teamTwoBotsText.text = "Bots: " + input;
		}

		public void OnChangeT2Difficulty(int input)
		{
			game.TeamTwoDifficulty = (AI.Difficulty)input;
		}

		public void OnChangeMatchDuration(float input)
		{
			game.MatchLength = input;
			SetMatchDurationText(input);
		}
		private void SetMatchDurationText(float input)
		{
			matchDurationText.text = "Match Duration: " + input;
		}
	}
}
