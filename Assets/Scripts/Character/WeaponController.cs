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
		[SerializeField] bool controllerDealsDamage = false;
		[SerializeField] Weapon[] weaponPrefabs;
		[ReadOnlyField] Weapon currentWeapon;

		//Properties
		public bool isSwappingWeapons => swapWeaponTimer > 0;
		public bool currentWeaponHasAmmo => currentWeapon.canFire;

		//Events
		public UnityEvent onChangeWeapon;

		//Members
		List<Weapon> weapons = new List<Weapon>();
		int currentWeaponIndex = 0;
		Camera mainCamera;
		AimController ac;
		PlayerInputRelay input;
		Unit unit;
		float swapWeaponTimer;


		void Awake()
		{
			mainCamera = Camera.main;
			input = GetComponent<PlayerInputRelay>();
			unit = GetComponent<Unit>();
			ac = GetComponent<AimController>();
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
			if (input.fire) FireWeapon();
			if (input.reload) ReloadWeapon();
			HandleWeaponSwitching();
			HandleTiming();
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


		public void FireWeapon()    //AI friendly
		{
			if (isSwappingWeapons) return;

			//TEMP: Because the animation rigging system is faulty, we let this controller do damage instead
			currentWeapon?.Fire(dealsDamage: !controllerDealsDamage);

			if (ac.isAiming && controllerDealsDamage && ac.target.HasValue)
			{
				//Deal damage via hit scan
				var damageable = ac.target.Value.collider.GetComponent<IDamageable>();
				if (damageable != null)
				{
					damageable.TakeDamage(currentWeapon.damage);
				}

				//hit particle
				if (currentWeapon.hitPFX != null)
				{
					var particle = Instantiate(currentWeapon.hitPFX, ac.target.Value.point, ac.target.Value.transform.rotation);
					Destroy(particle, 1f);  //BAD
				}
			}
		}

		public void ReloadWeapon()  //AI friendly
		{
			if (currentWeapon.isReloading) return;
			currentWeapon?.Reload();
		}

		void HandleWeaponSwitching()
		{
			if (input.nextWeapon) NextWeapon();
			else if (input.prevWeapon) PreviousWeapon();
		}

		public void NextWeapon()    //AI friendly
		{
			if (weapons.Count == 0) return;

			currentWeaponIndex++;
			if (currentWeaponIndex > weapons.Count - 1)
				currentWeaponIndex = 0;

			SetWeapon(currentWeaponIndex);
			onChangeWeapon.Invoke();
		}

		public void PreviousWeapon()    //AI friendly
		{
			if (weapons.Count == 0) return;

			currentWeaponIndex--;
			if (currentWeaponIndex < 0)
				currentWeaponIndex = weapons.Count - 1;

			SetWeapon(currentWeaponIndex);
			onChangeWeapon.Invoke();
		}

		void HandleTiming()
		{
			if (swapWeaponTimer > 0)
				swapWeaponTimer -= Time.deltaTime;
		}
	}
}
