using System;
using LeMinhHuy.Controllers;
using UnityEngine;
using UnityEngine.AI;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace LeMinhHuy.Character
{
	[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
	[RequireComponent(typeof(PlayerInput))]
#endif
	public class ThirdPersonController : MonoBehaviour
	{
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		public float WalkSpeed = 3f;

		[Tooltip("Sprint speed of the character in m/s")]
		public float SprintSpeed = 8f;
		public float CrouchSpeed = 1f;

		[Tooltip("How fast the character turns to face movement direction")]
		[Range(0.0f, 0.3f)]
		public float RotationSmoothTime = 0.12f;

		[Tooltip("Acceleration and deceleration")]
		public float SpeedChangeRate = 10.0f;

		[Space(10)]
		[Tooltip("The height the player can jump")]
		public float JumpHeight = 1.2f;

		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float Gravity = -15.0f;

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float JumpTimeout = 0.50f;

		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float FallTimeout = 0.15f;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool Grounded = true;

		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = -0.14f;

		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.28f;

		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject CinemachineCameraTarget;

		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 70.0f;

		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -30.0f;

		[Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
		public float CameraAngleOverride = 0.0f;

		[Tooltip("For locking the camera position on all axis")]
		public bool LockCameraPosition = false;

		[SerializeField] float crouchLayerWeight = 0.5f;

		// cinemachine
		private float cinemachineTargetYaw;
		private float cinemachineTargetPitch;

		// player
		float speed;
		float animationBlend;
		float targetRotation = 0.0f;
		float rotationVelocity;
		float verticalVelocity;
		float terminalVelocity = 53.0f;

		// timeout deltatime
		float jumpTimeoutDelta;
		float fallTimeoutDelta;

		// animation IDs
		const int crouchLayerID = 2;
		int hSpeedX;
		int hSpeedZ;
		int hGrounded;
		int hJump;
		int hFreeFall;
		int hMotionSpeed;
		const float _threshold = 0.01f;

		//Properites
		bool IsCurrentDeviceMouse
		{
			get
			{
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
				return playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
			}
		}
		bool hasAnimator => a != null;
		bool hasNavMeshAgent => agent != null;
		bool hasCinemachineCameraTarget => CinemachineCameraTarget != null;

		//Members
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		PlayerInput playerInput;
#endif
		PlayerInputRelay input;
		Animator a;
		CharacterController controller;
		NavMeshAgent agent;

		GameObject mainCamera;
		float aimSensitivity;
		bool overrideFacing = false;
		Transform t;
		float localSpeedXLerp;
		float localSpeedZLerp;
		float crouchSmoothWeight;
		private Vector3 targetDirection;

		void Awake()
		{
			t = transform;
			a = GetComponent<Animator>();
			controller = GetComponent<CharacterController>();
			input = GetComponent<PlayerInputRelay>();
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
			playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif
			agent = GetComponent<NavMeshAgent>();

			mainCamera = Camera.main.gameObject;
		}

		void Start()
		{
			AssignAnimationIDs();

			//Set starting rotation
			if (hasCinemachineCameraTarget)
				cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

			// reset our timeouts on start
			jumpTimeoutDelta = JumpTimeout;
			fallTimeoutDelta = FallTimeout;

			//TEMP: Legacy thing from the starter assets
			if (hasAnimator) a.SetFloat(hMotionSpeed, 1f);
		}
		void AssignAnimationIDs()
		{
			hSpeedX = Animator.StringToHash("SpeedX");
			hSpeedZ = Animator.StringToHash("SpeedZ");
			hGrounded = Animator.StringToHash("Grounded");
			hJump = Animator.StringToHash("Jump");
			hFreeFall = Animator.StringToHash("FreeFall");
			hMotionSpeed = Animator.StringToHash("MotionSpeed");
		}

		void Update()
		{
			HandleJumpAndGravity();
			HandleCrouching();
			HandleGroundCheck();
			HandleMovement();
		}

		void LateUpdate()
		{
			HandleCameraRotation();
			ResetAim();
		}
		//Run right at the end in order to reset the sensitivity for the next frame
		void ResetAim()
		{
			aimSensitivity = 1f;
			overrideFacing = false;
		}

		void HandleGroundCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(t.position.x, t.position.y - GroundedOffset, t.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

			// update animator if using character
			if (hasAnimator)
			{
				a.SetBool(hGrounded, Grounded);
			}
		}

		//Should only override for the current frame
		public void OverrideAimSensitivity(float overrideSensitivity) => aimSensitivity = overrideSensitivity;
		//Smoothly lerp toward a certain direction
		public void SetTargetLookDirection(Vector3 faceDirection, float lerpFactor = 30f)
		{
			overrideFacing = true;

			t.forward = Vector3.Lerp(t.forward, faceDirection, Time.deltaTime * lerpFactor);
		}

		//Delete me
		public void SetTargetLookAngleWithOffset(float desiredAngle, float lerpFactor = 20f)
		{
			overrideFacing = true;

			const float angleOffset = 10f;
			var yAng = t.rotation.eulerAngles.y;
			yAng = Mathf.LerpAngle(yAng, desiredAngle + angleOffset, Time.deltaTime * lerpFactor);
			t.rotation = Quaternion.Euler(0, yAng, 0);
		}

		void HandleCameraRotation()
		{
			if (!hasCinemachineCameraTarget) return;

			// if there is an input and camera position is not fixed
			if (input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
			{
				//Don't multiply mouse input by Time.deltaTime;
				float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

				cinemachineTargetYaw += input.look.x * aimSensitivity * deltaTimeMultiplier;
				cinemachineTargetPitch += input.look.y * aimSensitivity * deltaTimeMultiplier;
			}

			// clamp our rotations so our values are limited 360 degrees
			cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
			cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, BottomClamp, TopClamp);

			// Cinemachine will follow this target
			CinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch + CameraAngleOverride, cinemachineTargetYaw, 0.0f);
		}

		void HandleCrouching()
		{
			if (!hasAnimator) return;

			if (input.crouch)
			{
				crouchSmoothWeight = Mathf.Lerp(crouchSmoothWeight, crouchLayerWeight, Time.deltaTime * 20f);
				a.SetLayerWeight(crouchLayerID, crouchSmoothWeight);
			}
			else
			{
				crouchSmoothWeight = Mathf.Lerp(crouchSmoothWeight, 0, Time.deltaTime * 20f);
				a.SetLayerWeight(crouchLayerID, crouchSmoothWeight);
			}
		}

		void HandleMovement()   //takes in a vector3 desiredVelocity?
		{
			const float deadzone = 0.1f;
			float desiredSpeed = 0;// = input.sprint ? SprintSpeed : MoveSpeed;

			if (hasNavMeshAgent)
			{
				targetDirection = agent.desiredVelocity;
				desiredSpeed = agent.speed;     //Slow
			}
			else
			{
				//Set the desired speed
				if (input.crouch)
				{
					desiredSpeed = CrouchSpeed;
				}
				else if (input.sprint)
				{
					desiredSpeed = SprintSpeed;
				}
				else
				{
					desiredSpeed = WalkSpeed;
				}

				//Deal with input deadzone
				if (input.move == Vector2.zero) desiredSpeed = 0.0f;  //NOTE: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude

				// a reference to the players current horizontal velocity
				float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

				float inputMagnitude = input.analogMovement ? input.move.magnitude : 1f;

				//Smooth lerp to target speed
				if (currentHorizontalSpeed < desiredSpeed - deadzone || currentHorizontalSpeed > desiredSpeed + deadzone)
				{
					// creates curved result rather than a linear one giving a more organic speed change
					// note T in Lerp is clamped, so we don't need to clamp our speed
					speed = Mathf.Lerp(currentHorizontalSpeed, desiredSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

					// round speed to 3 decimal places
					speed = Mathf.Round(speed * 1000f) / 1000f;
				}
				else
				{
					speed = desiredSpeed;
				}

				//normalise input direction
				Vector3 inputDirection = new Vector3(input.move.x, 0.0f, input.move.y).normalized;

				//Calculate target facing
				// if there is a move input rotate player when the player is moving
				if (!overrideFacing && input.move != Vector2.zero)
				{
					targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
					float rotation = Mathf.SmoothDampAngle(t.eulerAngles.y, targetRotation, ref rotationVelocity, RotationSmoothTime);

					// rotate to face input direction relative to camera position
					t.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
				}

				//Player controlled
				targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
				controller.Move(targetDirection.normalized * (speed * Time.deltaTime) + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
			}

			//UPDATE ANIMATOR
			if (hasAnimator)
			{
				var localSpeedX = Vector3.Dot(targetDirection.normalized, t.right) * desiredSpeed;
				if (localSpeedX > -deadzone && localSpeedX < deadzone) localSpeedX = 0;
				localSpeedXLerp = Mathf.Lerp(localSpeedXLerp, localSpeedX, Time.deltaTime * SpeedChangeRate);

				var localSpeedZ = Vector3.Dot(targetDirection.normalized, t.forward) * desiredSpeed;
				if (localSpeedZ > -deadzone && localSpeedZ < deadzone) localSpeedZ = 0;
				localSpeedZLerp = Mathf.Lerp(localSpeedZLerp, localSpeedZ, Time.deltaTime * SpeedChangeRate);

				a.SetFloat(hSpeedX, localSpeedXLerp);
				a.SetFloat(hSpeedZ, localSpeedZLerp);
				// animator.SetFloat(hMotionSpeed, inputMagnitude);        //TEMP: What does this do exactly?
				// print($"tdn: {targetDirection.normalized}, ls: ({localSpeedX}, {localSpeedZ}), ds: {desiredSpeed}, lsl: ({localSpeedXLerp}, {localSpeedZLerp})");
			}
		}

		void HandleJumpAndGravity()
		{
			if (Grounded)
			{
				// reset the fall timeout timer
				fallTimeoutDelta = FallTimeout;

				// update animator if using character
				if (hasAnimator)
				{
					a.SetBool(hJump, false);
					a.SetBool(hFreeFall, false);
				}

				// stop our velocity dropping infinitely when grounded
				if (verticalVelocity < 0.0f)
				{
					verticalVelocity = -2f;
				}

				// Jump
				if (input.jump && jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

					// update animator if using character
					if (hasAnimator)
					{
						a.SetBool(hJump, true);
					}
				}

				// jump timeout
				if (jumpTimeoutDelta >= 0.0f)
				{
					jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				jumpTimeoutDelta = JumpTimeout;

				// fall timeout
				if (fallTimeoutDelta >= 0.0f)
				{
					fallTimeoutDelta -= Time.deltaTime;
				}
				else
				{
					// update animator if using character
					if (hasAnimator)
					{
						a.SetBool(hFreeFall, true);
					}
				}

				// if we are not grounded, do not jump
				input.SetJump(false);
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (verticalVelocity < terminalVelocity)
			{
				verticalVelocity += Gravity * Time.deltaTime;
			}
		}

		static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (Grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;

			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
		}
	}
}