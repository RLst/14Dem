using LeMinhHuy.Input;
using UnityEngine;

namespace StarterAssets
{
	public class UICanvasControllerInput : MonoBehaviour
	{

		[Header("Output")]
		public PlayerInputReadOnly playerInput;

		public void VirtualMoveInput(Vector2 virtualMoveDirection)
		{
			playerInput.MoveInput(virtualMoveDirection);
		}

		public void VirtualLookInput(Vector2 virtualLookDirection)
		{
			playerInput.LookInput(virtualLookDirection);
		}

		public void VirtualJumpInput(bool virtualJumpState)
		{
			playerInput.JumpInput(virtualJumpState);
		}

		public void VirtualSprintInput(bool virtualSprintState)
		{
			playerInput.SprintInput(virtualSprintState);
		}

	}

}
