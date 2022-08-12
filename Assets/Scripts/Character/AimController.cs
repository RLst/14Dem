using Cinemachine;
using LeMinhHuy.Input;
using StarterAssets;
using UnityEngine;

namespace LeMinhHuy.AI
{
	//Controls aim mode; zoom aim sight and reduce aim sensitivity
	//Controls player orientation during aiming
	public class AimController : MonoBehaviour
	{
		[SerializeField] CinemachineVirtualCamera aimCamera;

		[SerializeField] float aimSensitivity = 0.4f;

		PlayerInputRelay input;
		ThirdPersonController tpc;
		WeaponController sc;
		Transform t;    //Cache to improve performance as transform is extern call

		void Awake()
		{
			input = GetComponent<PlayerInputRelay>();
			tpc = GetComponent<ThirdPersonController>();
			sc = GetComponent<WeaponController>();
			t = transform;
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
			}
		}
	}
}
