using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace LeMinhHuy.AI
{
	public class Unit : MonoBehaviour, IDamageable
	{
		public float maxHealth = 100;
		public float health;
		public Team team = Team.South;

		[Header("Events")]
		public UnityEvent onKill;

		public void TakeDamage(float damage)
		{
			health -= damage;
			if (health < 0)
				Kill();
		}

		public void Kill()
		{
			//Activate ragdoll
			//apply bullet force to body
			//disable animators
			//

			onKill.Invoke();
		}
	}
}
