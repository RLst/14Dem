using LeMinhHuy.Input;
using UnityEngine;

namespace StarterAssets
{
	public class UICanvasControllerInput : MonoBehaviour
	{
		[Header("Output")]
		public PlayerInputRelay input;

		public void VirtualMoveInput(Vector2 virtualMoveDirection)
		{
			input.SetMove(virtualMoveDirection);
		}

		public void VirtualLookInput(Vector2 virtualLookDirection)
		{
			input.SetLook(virtualLookDirection);
		}

		public void VirtualJumpInput(bool virtualJumpState)
		{
			input.SetJump(virtualJumpState);
		}

		public void VirtualSprintInput(bool virtualSprintState)
		{
			input.SetSprint(virtualSprintState);
		}
	}
}
