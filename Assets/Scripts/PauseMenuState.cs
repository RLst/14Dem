using UnityEngine;

namespace LeMinhHuy
{
	/// <summary>
	/// Auto handles game timescale changes
	/// </summary>
	public class PauseMenuState : CoverState
	{
		[Space(10), SerializeField] float pauseTimeScale = 0.2f;

		public override void OnEnter(UltraStateMachine sm)
		{
			base.OnEnter(sm);

			Cursor.lockState = CursorLockMode.None;
			// Cursor.visible = true;
			Time.timeScale = pauseTimeScale;
		}

		public override void OnExit(UltraStateMachine sm)
		{
			base.OnExit(sm);

			Cursor.lockState = CursorLockMode.Locked;
			// Cursor.visible = false;
			Time.timeScale = 1f;
		}
	}
}
