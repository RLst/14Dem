using LeMinhHuy.AI.Core;
using LeMinhHuy.Events;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace LeMinhHuy.Character
{
	[SelectionBase]
	public class Unit : MonoBehaviour, IHealth, ICoreDamageable, IKillable
	{
		//Inspector
		[SerializeField] float maxHealth = 100;
		[field: SerializeField] public float health { get; set; }
		public Team team = Team.South;
		public Transform weaponMount;
		[SerializeField] SkinnedMeshRenderer[] unitSkins;

		//Properties
		public bool isAIControlled => behaviourAgent != null;
		internal Team opposingTeam
		{
			get
			{
				return team == Team.South ? Team.North : Team.South;
			}
		}

		[Header("Events")]
		// public UnitEvent onKill;

		//Members
		BehaviourAgent behaviourAgent;

		//TODO: Put damageables on limbs
		public void TakeCoreDamage(float damage)
		{
			health -= damage;
			if (health < 0)
				Kill();
		}

		public void Kill()
		{
			// onKill.Invoke(this);
			//Activate ragdoll
			//apply bullet force to body
			//disable animators
			//TEMP
			Destroy(this.gameObject);
		}

		void Awake()
		{
			behaviourAgent = GetComponent<BehaviourAgent>();
		}

		void Start()
		{
			Debug.Assert(weaponMount != null, "MUST set unit's weapon mount point (gun holding hand)");
			health = maxHealth;
			ChooseRandomSkin();
		}

		public void ChooseRandomSkin()
		{
			foreach (var s in unitSkins)
			{
				s.gameObject.SetActive(false);
			}
			unitSkins[Random.Range(0, unitSkins.Length)].gameObject.SetActive(true);
		}
	}
}
