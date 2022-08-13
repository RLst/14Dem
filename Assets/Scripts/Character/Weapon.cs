using UnityEngine;
using UnityEngine.Events;

namespace LeMinhHuy.Character
{
	public class Weapon : MonoBehaviour
	{
		//Inspector
		[field: SerializeField] public Transform muzzle { get; private set; }
		[SerializeField] int maxAmmoCapacity = 30;
		[SerializeField] float damage = 5f;
		[Tooltip("in RPM")]
		[SerializeField] float fireRate = 500f;
		float timeBetweenShots;
		[Tooltip("in seconds")]
		[SerializeField] float reloadTime = 1f;
		[Tooltip("in metres")]
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

			//Hitscan damage
			if (Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hit, range, shootableLayerMask))
			{
				var damageable = hit.collider.GetComponent<IDamageable>();
				if (damageable is object)
				{
					damageable.TakeDamage(damage);
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
