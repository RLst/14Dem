using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace LeMinhHuy.Character
{
	[SelectionBase]
	public class Unit : MonoBehaviour, IHealth, ICoreDamageable, IKillable
	{
		[SerializeField] float maxHealth = 100;
		[field: SerializeField] public float health { get; set; }

		public Team team = Team.South;
		public Transform weaponMount;

		[Header("Events")]
		public UnityEvent onKill;

		//Properties
		bool hasAgent => agent != null;

		//Members
		NavMeshAgent agent;

		public void TakeCoreDamage(float damage)
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

		void Start()
		{
			Debug.Assert(weaponMount != null, "MUST set unit's weapon mount point (gun holding hand)");
			health = maxHealth;
		}
	}
}
