using Cinemachine;
using LeMinhHuy.Input;
using StarterAssets;
using UnityEngine;

namespace LeMinhHuy.AI
{
	public class AimZoomController : MonoBehaviour
	{
		//Zooms in and reduces the aim sensitivity
		[SerializeField] CinemachineVirtualCamera aimCamera;

		[SerializeField] float aimSensitivity = 0.4f;

		PlayerInputRelay input;
		ThirdPersonController tpc;

		void Awake()
		{
			input = GetComponent<PlayerInputRelay>();
			tpc = GetComponent<ThirdPersonController>();
		}

		void Update()
		{
			if (input.aim)
			{
				// aimCamera.gameObject.SetActive(true);
				tpc.OverrideAimSensitivity(aimSensitivity);
			}
			aimCamera.gameObject.SetActive(input.aim);
		}
	}
}
