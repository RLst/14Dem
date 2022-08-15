using System.Linq;
using LeMinhHuy.AI.Core;
using LeMinhHuy.Character;
using UnityEngine;

namespace LeMinhHuy.AI
{
	[RequireComponent(typeof(Unit))]
	public abstract class BotAgent : BehaviourAgent
	{
		public Unit[] allUnits;
		public Unit[] allies;  //units of the same team
		public Unit[] enemies;
		public Unit unit;
		protected WeaponController weaponController;
		protected AimController aimController;

		protected override void Awake()
		{
			weaponController = GetComponent<WeaponController>();
			aimController = GetComponent<AimController>();

			base.Awake();
		}

		protected override void Start()
		{
			base.Start();

			unit = GetComponent<Unit>();
			allUnits = FindObjectsOfType<Unit>();
			allies = allUnits.Where(x => x.team == this.unit.team).ToArray();
			enemies = allUnits.Where(x => x.team == this.unit.opposingTeam).ToArray();
		}

		protected virtual void Retreat()
		{

		}
		protected virtual void Wander()
		{
			// var wander = new Vector3(Random.Range(-wanderDist, wanderDist), 0, Random.Range(-wanderDist, wanderDist));
			// var destination = transform.position + wander;
			// agent.SetDestination(destination);
		}
		protected virtual void Chase()
		{

		}
	}
}