using Cinemachine;
using LeMinhHuy.Input;
using StarterAssets;
using UnityEngine;

namespace LeMinhHuy.Character
{
	//Controls aim mode; zoom aim sight and reduce aim sensitivity
	//Controls player orientation during aiming
	public class AimController : MonoBehaviour
	{
		[SerializeField] CinemachineVirtualCamera aimCamera;

		[SerializeField] float aimSensitivity = 0.4f;
		[SerializeField] float aimLayerWeight = 0.5f;   //Animator layer weight

		PlayerInputRelay input;
		ThirdPersonController tpc;
		WeaponController sc;
		Animator a;
		Transform t;    //Cache to improve performance as transform is extern call

		bool hasAnimator => a is object;
		int hAim;
		private float smoothWeight;

		void Awake()
		{
			t = transform;
			input = GetComponent<PlayerInputRelay>();
			tpc = GetComponent<ThirdPersonController>();
			sc = GetComponent<WeaponController>();
			a = GetComponent<Animator>();
		}

		void Start()
		{
			hAim = Animator.StringToHash("Aim");
		}

		void Update()
		{
			aimCamera.gameObject.SetActive(input.aim);

			if (input.aim)
			{
				tpc.OverrideAimSensitivity(aimSensitivity);

				//Handle locked player facing while aiming
				Vector3 yEqualizedAimWorldPos = sc.aimWorldPosition;
				yEqualizedAimWorldPos.y = t.position.y;
				var faceDirection = (yEqualizedAimWorldPos - t.position).normalized;
				tpc.LerpForwardFacing(faceDirection);

				if (hasAnimator)
				{
					// smoothWeight = Mathf.Lerp(smoothWeight, aimLayerWeight, Time.deltaTime * 20f);
					a.SetBool(hAim, true);
				}
			}
			else
			{
				if (hasAnimator)
				{
					// smoothWeight = Mathf.Lerp(smoothWeight, 0, Time.deltaTime * 20f);
					a.SetBool(hAim, false);
				}
			}
		}
	}
}
