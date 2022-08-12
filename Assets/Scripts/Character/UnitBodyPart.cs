using UnityEngine;

namespace LeMinhHuy.Character
{
	//Attach to unit body parts + colliders downstream
	//Sends damage to the core damage
	public class UnitBodyPart : MonoBehaviour, IDamageable
	{
		[Range(0f, 10f)]
		[SerializeField] float damageMultiplier = 1f;   //ie. Limbs = 0.6, Body = 1, Head = 4
		ICoreDamageable coreDamageable;

		void Awake()
		{
			coreDamageable = GetComponentInParent<ICoreDamageable>();
		}

		public void TakeDamage(float damage)
		{
			//Multiply damage
			coreDamageable.TakeCoreDamage(damage * damageMultiplier);
			// SendMessageUpwards("TakeMainDamage", damage * damageMultiplier);
		}
	}
}
