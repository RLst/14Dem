using System;
using LeMinhHuy.Input;
using UnityEngine;

namespace LeMinhHuy.AI
{
	[RequireComponent(typeof(Unit), typeof(PlayerInputRelay))]
	public class WeaponController : MonoBehaviour    //Rename to weapon controller
	{
		[SerializeField] LayerMask aimLayerMask;

		[SerializeField] Weapon[] weapons;
		[SerializeField] Transform unitWeaponMount;
		Weapon currentWeapon;
		int currentWeaponIndex = 0;

		//Properties
		public Vector3 aimWorldPosition { get; private set; }

		//Members
		Camera mainCam;
		Transform position;
		PlayerInputRelay input;
		Unit unit;

		void Awake()
		{
			mainCam = Camera.main;
			input = GetComponent<PlayerInputRelay>();
			unit = GetComponent<Unit>();
		}

		void Start()
		{
			//Set a current weapon
			if (weapons.Length == 0) return;
			currentWeapon = weapons[0];

			MountAllWeapons();
		}

		void Update()
		{
			HandleWeaponTargeting();
			HandleWeaponFiring();
			HandleWeaponReloading();
			HandleWeaponSwitching();
		}

		void HandleWeaponTargeting()
		{
			if (input.aim)
			{
				aimWorldPosition = Vector3.zero;
				var screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
				var shootRay = mainCam.ScreenPointToRay(screenCenter);
				if (Physics.Raycast(shootRay, out RaycastHit hit, 500f, aimLayerMask))
				{
					aimWorldPosition = hit.point;
				}
			}
		}

		void HandleWeaponFiring()
		{
			if (input.shoot)
			{
				currentWeapon.Fire();
			}
		}

		void HandleWeaponReloading()
		{
			if (input.reload)
			{
				currentWeapon.Reload();
			}
		}

		void HandleWeaponSwitching()
		{
			if (input.nextWeapon) NextWeapon();
			else if (input.prevWeapon) PreviousWeapon();
		}

		void MountAllWeapons()
		{
			foreach (var w in weapons)
			{
				w.transform.SetParent(unit.weaponMount);
				w.gameObject.SetActive(false);
			}
		}
		void SetWeapon(int index)
		{
			try
			{
				foreach (var w in weapons)
					w.gameObject.SetActive(false);
				weapons[index].gameObject.SetActive(true);
			}
			catch (Exception e)
			{
				Debug.LogWarning(e, this);
			}
		}

		void NextWeapon()
		{
			if (weapons.Length == 0) return;

			currentWeaponIndex++;
			if (currentWeaponIndex > weapons.Length)
				currentWeaponIndex = 0;

			SetWeapon(currentWeaponIndex);
		}

		void PreviousWeapon()
		{
			if (weapons.Length == 0) return;

			currentWeaponIndex--;
			if (currentWeaponIndex < 0)
				currentWeaponIndex = weapons.Length;

			SetWeapon(currentWeaponIndex);
		}
	}
}
