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
		[field: SerializeField] public bool shoot { get; private set; }
		[field: SerializeField] public bool nextWeapon { get; private set; }
		[field: SerializeField] public bool prevWeapon { get; private set; }

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		//Messages sent by PlayerInput
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
		public void OnShoot(InputValue value) => SetShoot(value.isPressed);
		public void OnNextWeapon(InputValue value) => SetNextWeapon(value.isPressed);
		public void OnPrevWeapon(InputValue value) => SetPrevWeapon(value.isPressed);
#endif

		//Set functions can be accessed from outside
		public void SetLook(Vector2 input) => look = input;
		public void SetAim(bool input) => aim = input;
		public void SetMove(Vector2 input) => move = input;
		public void SetSprint(bool input) => sprint = input;
		public void SetJump(bool input) => jump = input;
		public void SetShoot(bool input) => shoot = input;
		public void SetNextWeapon(bool input) => nextWeapon = input;
		public void SetPrevWeapon(bool input) => prevWeapon = input;


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