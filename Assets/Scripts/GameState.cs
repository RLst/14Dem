namespace LeMinhHuy
{
	//In-game UI state
	public class GameState : CoverState
	{
		GameController game;

		void Awake()
		{
			game = FindObjectOfType<GameController>();
		}

		public override void OnCover(UltraStateMachine sm)
		{
			base.OnCover(sm);
		}
	}
}
