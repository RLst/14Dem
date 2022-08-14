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
		[SerializeField] CinemachineVirtualCamera aimCamera;
		[Space]
		[SerializeField] float aimSensitivity = 0.4f;
		[SerializeField] float aimLayerWeight = 0.5f;   //Animator layer weight
		[SerializeField] float aimRotationOffset = 35f;
		[SerializeField] float aimSmoothing = 50f;

		//Properties
		public bool isAiming { get; private set; }
		bool hasAnimator => a != null;

		//Members
		PlayerInputRelay input;
		ThirdPersonController tpc;
		WeaponController weaponController;
		Animator a;
		Transform t;    //Cache to improve performance as transform is extern call
		int hAim;

		void Awake()
		{
			t = transform;
			input = GetComponent<PlayerInputRelay>();
			tpc = GetComponent<ThirdPersonController>();
			weaponController = GetComponent<WeaponController>();
			a = GetComponent<Animator>();
		}

		void Start()
		{
			Debug.Assert(a != null, "Animator not found");
			hAim = Animator.StringToHash("Aim");
		}

		void Update()
		{
			aimCamera.gameObject.SetActive(input.aim);

			if (input.aim)
			{
				Aim();
			}
			else
			{
				if (hasAnimator)
				{
					a.SetBool(hAim, false);
				}
			}
		}

		void Aim()
		{
			tpc.OverrideAimSensitivity(aimSensitivity);

			//Handle locked player facing while aiming
			Vector3 aimWorldPositionFlattened = weaponController.aimWorldPosition;
			aimWorldPositionFlattened.y = t.position.y;
			Vector3 lookDirection = (aimWorldPositionFlattened - t.position).normalized;

			//Apply offset
			var quat = Quaternion.Euler(0, aimRotationOffset, 0);
			lookDirection = quat * lookDirection;

			tpc.SetTargetLookDirection(lookDirection, aimSmoothing);

			if (hasAnimator)
			{
				a.SetBool(hAim, true);
			}
		}
	}
}
