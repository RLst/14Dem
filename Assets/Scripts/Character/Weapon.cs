using UnityEngine;
using UnityEngine.Events;

namespace LeMinhHuy.AI
{
	public class Weapon : MonoBehaviour
	{
		//Inspector
		[SerializeField] Transform muzzle;
		[SerializeField] int maxAmmoCapacity = 30;
		[SerializeField] float damage = 5f;
		[Tooltip("in RPM")]
		[SerializeField] float fireRate = 300f;
		float fireRatePerSecond => fireRate * 0.0166666666f;   //1/60
		[Tooltip("in seconds")]
		[SerializeField] float reloadTime = 1f;
		[Tooltip("in metres")]
		[SerializeField] float range = 50f;
		[SerializeField] LayerMask shootableLayerMask;

		//Events
		public UnityEvent onReload;
		public UnityEvent onEmptyShoot;

		//Properties
		bool canReload => reloadTimer <= 0;

		//Members
		int ammo;
		float reloadTimer;

		public bool TrySpendAmmo()
		{
			if (ammo > 0)
			{
				--ammo;
				return true;
			}
			return false;
		}

		public void Reload()
		{
			if (!canReload) return;

			ammo = maxAmmoCapacity;
			reloadTimer = reloadTime;

			onReload.Invoke();
		}

		public void Fire()
		{
			if (!TrySpendAmmo())
			{
				onEmptyShoot.Invoke();    //Click click!
				return;
			}

			if (Physics.Raycast(muzzle.position, muzzle.forward, out RaycastHit hit, range, shootableLayerMask))
			{
				var damageable = hit.collider.GetComponent<IDamageable>();
				if (damageable is object)
				{
					damageable.TakeDamage(damage);
				}
			}
		}

		void Update()
		{
			if (reloadTimer > 0)
				reloadTimer -= Time.deltaTime;
		}
	}
}
