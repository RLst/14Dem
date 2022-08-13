using System;
using System.Collections.Generic;
using LeMinhHuy.Controllers;
using UnityEngine;
using UnityEngine.Events;

namespace LeMinhHuy.Character
{
	[RequireComponent(typeof(Unit), typeof(PlayerInputRelay))]
	public class WeaponController : MonoBehaviour    //Rename to weapon controller
	{
		[SerializeField] LayerMask aimLayerMask;
		[SerializeField] Weapon[] weaponPrefabs;

		//Properties
		public Vector3 aimWorldPosition { get; private set; }

		//Events
		public UnityEvent onChangeWeapon;

		//Members
		Weapon currentWeapon;
		List<Weapon> weapons = new List<Weapon>();
		int currentWeaponIndex = 0;
		Camera mainCamera;
		Transform position;
		PlayerInputRelay input;
		Unit unit;

		void Awake()
		{
			mainCamera = Camera.main;
			input = GetComponent<PlayerInputRelay>();
			unit = GetComponent<Unit>();
		}

		void Start()
		{
			//Set a current weapon
			if (weaponPrefabs.Length == 0) return;
			MountAllWeapons();
			SetWeapon(0);
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
				var shootRay = mainCamera.ScreenPointToRay(screenCenter);
				if (Physics.Raycast(shootRay, out RaycastHit hit, 500f, aimLayerMask))
				{
					aimWorldPosition = hit.point;
				}
			}
		}

		void HandleWeaponFiring()
		{
			if (input.fire)
			{
				currentWeapon?.Fire();
			}
		}

		void HandleWeaponReloading()
		{
			if (input.reload)
			{
				currentWeapon?.Reload();
			}
		}

		void HandleWeaponSwitching()
		{
			if (input.nextWeapon) NextWeapon();
			else if (input.prevWeapon) PreviousWeapon();
		}

		void MountAllWeapons()
		{
			foreach (var w in weaponPrefabs)
			{
				var newWeapon = Instantiate<Weapon>(w, unit.weaponMount.position, unit.weaponMount.rotation, unit.weaponMount);
				newWeapon.gameObject.SetActive(false);
				weapons.Add(newWeapon);
			}
		}
		void SetWeapon(int index)
		{
			try
			{
				foreach (var w in weapons)
					w.gameObject.SetActive(false);
				currentWeapon = weapons[index];
				currentWeapon.gameObject.SetActive(true);
				currentWeapon.Reload();
			}
			catch (Exception e)
			{
				Debug.LogWarning(e, this);
			}
		}

		void NextWeapon()
		{
			if (weapons.Count == 0) return;

			currentWeaponIndex++;
			if (currentWeaponIndex > weapons.Count)
				currentWeaponIndex = 0;

			SetWeapon(currentWeaponIndex);
			onChangeWeapon.Invoke();
		}

		void PreviousWeapon()
		{
			if (weapons.Count == 0) return;

			currentWeaponIndex--;
			if (currentWeaponIndex < 0)
				currentWeaponIndex = weapons.Count;

			SetWeapon(currentWeaponIndex);
			onChangeWeapon.Invoke();
		}
	}
}
