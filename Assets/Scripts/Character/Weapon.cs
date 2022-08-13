using UnityEngine;
using UnityEngine.Events;

namespace LeMinhHuy.Character
{
	public class Weapon : MonoBehaviour
	{
		//Inspector
		[field: SerializeField] public Transform muzzle { get; private set; }
		[SerializeField] GameObject hitPFX;
		[SerializeField] GameObject gunFlashPFX;
		[SerializeField] int maxAmmoCapacity = 30;
		[SerializeField] float damage = 5f;
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
		bool canReload => ammo < maxAmmoCapacity && reloadTimer <= 0;
		bool isReloading => reloadTimer > 0;
		bool canFire => !isReloading && fireTimer <= 0;

		//Members
		int ammo;
		float timeBetweenShots;
		float reloadTimer;
		float fireTimer;

		void Start()
		{
			timeBetweenShots = 60f / fireRate;
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

		public void Fire()
		{
			if (!canFire) return;
			if (!TrySpendAmmo())
			{
				onEmptyMagazine.Invoke();    //Click click!
				return;
			}

			//Firing! Start fire timer
			fireTimer = timeBetweenShots;

			//Gun flash
			if (gunFlashPFX != null)
			{
				var particle = Instantiate(gunFlashPFX, muzzle.position, muzzle.rotation);
				Destroy(particle, 0.1f);    //TODO:
			}

			//Hitscan damage
			if (Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hit, range, shootableLayerMask))
			{
				// Debug.DrawRay(muzzle.position, muzzle.forward * range, Color.red, 20f);

				var damageable = hit.collider.GetComponent<IDamageable>();
				if (damageable != null)
				{
					damageable.TakeDamage(damage);
				}

				//hit particle
				if (hitPFX != null)
				{
					var particle = Instantiate(hitPFX, hit.point, hit.transform.rotation);
					Destroy(particle, 1f);  //BAD
				}
			}
			onFire.Invoke();
		}

		public void Reload()
		{
			if (!canReload) return;

			ammo = maxAmmoCapacity;
			reloadTimer = reloadTime;

			onReload.Invoke();
		}

		void Update()
		{
			//Time the reload
			if (reloadTimer > 0)
				reloadTimer -= Time.deltaTime;

			if (fireTimer > 0)
				fireTimer -= Time.deltaTime;
		}
	}
}
