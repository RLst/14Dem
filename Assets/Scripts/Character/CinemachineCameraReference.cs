using Cinemachine;
using UnityEngine;

namespace LeMinhHuy.Character
{
	public abstract class CinemachineCameraReference : MonoBehaviour
	{
		public Transform follow => cam.Follow;
		public Transform lookAt => cam.LookAt;
		public CinemachineVirtualCamera cam { get; private set; }
		void Awake()
		{
			cam = GetComponent<CinemachineVirtualCamera>();
		}
	}
}
