using LeMinhHuy.AI.Core;
using UnityEngine;

namespace LeMinhHuy.AI
{
	public abstract class BotAgent : BehaviourAgent
	{
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