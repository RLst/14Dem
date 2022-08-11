using System;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace LeMinhHuy.Input
{
	public class PlayerInputRelay : MonoBehaviour
	{
		// [Header("Character Input Values")]
		[field: SerializeField] public Vector2 look { get; private set; }
		[field: SerializeField] public bool aim { get; private set; }
		[field: SerializeField] public Vector2 move { get; private set; }
		[field: SerializeField] public bool sprint;
		[field: SerializeField] public bool jump { get; private set; }


		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnLook(InputValue value)
		{
			if (cursorInputForLook)
			{
				SetLook(value.Get<Vector2>());
			}
		}
		public void OnAim(InputValue value) => SetAim(value.isPressed);
		public void OnMove(InputValue value) => SetMove(value.Get<Vector2>());
		public void OnSprint(InputValue value) => SetSprint(value.isPressed);
		public void OnJump(InputValue value) => SetJump(value.isPressed);
#endif

		public void SetLook(Vector2 lookInput) => look = lookInput;
		public void SetAim(bool aimInput) => aim = aimInput;
		public void SetMove(Vector2 moveInput) => move = moveInput;
		public void SetSprint(bool sprintInput) => sprint = sprintInput;
		public void SetJump(bool jumpInput) => jump = jumpInput;

		void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}
		void SetCursorState(bool active)
		{
			Cursor.lockState = active ? CursorLockMode.Locked : CursorLockMode.None;
		}

	}

}