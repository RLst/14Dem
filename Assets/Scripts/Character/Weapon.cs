using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace LeMinhHuy.Character
{
	public class Weapon : MonoBehaviour
	{
		//Inspector
		[field: SerializeField] public Transform muzzle { get; private set; }
		public GameObject hitPFX;
		[SerializeField] GameObject gunFlashPFX;
		[SerializeField] float gunFlashDuration = 0.15f;
		[SerializeField] int maxAmmoCapacity = 30;
		[Tooltip("Damage done by one shot of this gun")]
		[field: SerializeField] public float damage { get; private set; } = 5f;
		[Tooltip("Fire rate in RPM")]
		[SerializeField] float fireRate = 1000f;
		[Tooltip("Reload time in seconds")]
		[SerializeField] float reloadTime = 1f;
		[Tooltip("Shoot range in metres")]
		[SerializeField] float range = 50f;
		[SerializeField] LayerMask shootableLayerMask;

		//Events
		public UnityEvent onFire;
		public UnityEvent onReload;
		public UnityEvent onEmptyMagazine;

		//Properties
		public bool canReload => ammo < maxAmmoCapacity && reloadTimer <= 0;
		public bool isReloading => reloadTimer > 0;
		public bool canFire => !isReloading && fireTimer <= 0;

		//Members
		int ammo;
		float r_timeBetweenShots;   //read only
		float reloadTimer;
		float fireTimer;

		void Start()
		{
			r_timeBetweenShots = 60f / fireRate;
			gunFlashPFX.SetActive(false);
		}
		void Update()
		{
			//Handle timings
			if (reloadTimer > 0)
				reloadTimer -= Time.deltaTime;

			if (fireTimer > 0)
				fireTimer -= Time.deltaTime;
		}

		public bool TrySpendAmmo()
		{
			if (ammo > 0)
			{
				--ammo;
				return true;
			}
			return false;
		}

		//Returns true if gun can be fired
		//TEMP: dont' return bool, really bad
		public bool Fire(bool dealsDamage = true)
		{
			if (!canFire) return false;
			if (!TrySpendAmmo())
			{
				onEmptyMagazine.Invoke();    //Click click!
				return false;
			}

			//Firing! Start fire timer
			fireTimer = r_timeBetweenShots;
			onFire.Invoke();

			//Gun flash
			if (gunFlashPFX != null)
			{
				StartCoroutine(GunFlash());
			}

			if (!dealsDamage) return true;

			//Weapon projected hitscan damage + hit particle generation
			if (Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hit, range, shootableLayerMask))
			{
				var damageable = hit.collider.GetComponent<IDamageable>();
				if (damageable != null)
				{
					damageable.TakeDamage(damage);
				}

				//hit particle
				if (hitPFX != null)
				{
					var particle = Instantiate(hitPFX, hit.point, hit.transform.rotation);
					print("weapon.fire");
					Destroy(particle, 1f);  //BAD
				}
			}
			return true;
		}

		public void Reload()
		{
			if (!canReload) return;

			ammo = maxAmmoCapacity;
			reloadTimer = reloadTime;

			onReload.Invoke();
		}

		IEnumerator GunFlash()
		{
			gunFlashPFX.SetActive(true);
			yield return new WaitForSeconds(gunFlashDuration);
			gunFlashPFX.SetActive(false);
		}
	}
}
