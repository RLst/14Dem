using Cinemachine;
using LeMinhHuy.Controllers;
using StarterAssets;
using UnityEngine;

namespace LeMinhHuy.Character
{
	//Controls aim mode; zoom aim sight and reduce aim sensitivity
	//Controls player orientation during aiming
	public class AimController : MonoBehaviour
	{
		//Inspector
		[SerializeField] float aimSensitivity = 0.4f;
		[SerializeField] float aimLayerWeight = 0.5f;   //Animator layer weight
		[SerializeField] float aimRotationOffset = 35f;
		[SerializeField] float aimSmoothing = 50f;
		[SerializeField] LayerMask aimLayerMask;
		[ReadOnlyField] AimCamera aimCamera;

		//Properties
		public bool isAiming
		{
			get
			{
				if (hasInput)
				{
					return input.aim;
				}
				return false;
			}
		}
		//Aim target world position; Where I'm aiming at
		public RaycastHit? target { get; private set; }
		bool hasAnimator => a != null;
		bool hasInput => input != null;

		//Members
		PlayerInputRelay input;
		ThirdPersonController tpc;
		WeaponController weaponController;
		Animator a;
		Transform t;    //Cache to improve performance as transform is extern call
		int hAim;
		Camera mainCam;
		private Vector3 lookDirection;
		Unit unit;


		void Awake()
		{
			unit = GetComponent<Unit>();
			t = transform;
			input = GetComponent<PlayerInputRelay>();
			tpc = GetComponent<ThirdPersonController>();
			weaponController = GetComponent<WeaponController>();
			a = GetComponent<Animator>();
			mainCam = Camera.main;

			if (!unit.isAIControlled) aimCamera = FindObjectOfType<AimCamera>();
		}

		void Start()
		{
			Debug.Assert(a != null, "Animator not found");
			hAim = Animator.StringToHash("Aim");
		}

		void Update()
		{
			if (!unit.isAIControlled)
				aimCamera.gameObject.SetActive(input.aim);

			if (!unit.isAIControlled && input.aim)
			{
				Aim();
			}
			else if (hasAnimator)
			{
				a.SetBool(hAim, false);
			}
		}

		public void Aim(Vector3? desiredTarget = null)
		{
			tpc.OverrideAimSensitivity(aimSensitivity);

			//Aim via hitscan
			const float RAYCAST_DIST = 1000f;
			var screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
			var shootRay = mainCam.ScreenPointToRay(screenCenter);
			if (Physics.Raycast(shootRay, out RaycastHit hit, RAYCAST_DIST, aimLayerMask))
			{
				target = hit;
			}
			target = null;

			//Face player correctly
			if (target.HasValue)
			{
				Vector3 aimWorldPositionFlattened = target.Value.point;
				aimWorldPositionFlattened.y = t.position.y;
				lookDirection = (aimWorldPositionFlattened - t.position).normalized;
			}
			else
			{
				Vector3 aimWorldPositionFlattened = mainCam.transform.forward;
				aimWorldPositionFlattened.y = t.position.y;
				lookDirection = aimWorldPositionFlattened;
			}

			//Apply slight rotational offset
			lookDirection = Quaternion.Euler(0, aimRotationOffset, 0) * lookDirection;

			//Set final character rotation
			tpc.SetTargetLookDirection(lookDirection, aimSmoothing);

			//Animator
			if (!hasAnimator) return;
			a.SetBool(hAim, true);
		}
	}
}
