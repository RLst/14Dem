using LeMinhHuy.AI.Core;
using UnityEngine;

namespace LeMinhHuy.AI
{
	public class Hunt : Action
	{
		float arriveDist;
		float wanderDist;
		float huntSpeed;

		public Hunt(float arriveDist = 1f, float huntSpeed = 3f, float huntDist = 5f)
		{
			this.arriveDist = arriveDist;
			this.huntSpeed = huntSpeed;
			this.wanderDist = huntDist;
		}

		public override NodeState OnExecute()
		{
			// Debug.Log("remain:" + owner.agent.remainingDistance);
			if (owner.enabled == false)
				return NodeState.Failure;

			owner.agent.speed = huntSpeed;

			if (owner.agent.hasPath == false || owner.agent.remainingDistance < arriveDist)
			{
				FindNewPath();
				return NodeState.Pending;
			}
			else
			{
				return NodeState.Pending;
			}
		}

		void FindNewPath()
		{
			var wander = new Vector3(Random.Range(-wanderDist, wanderDist), 0, Random.Range(-wanderDist, wanderDist));
			var destination = transform.position + wander;
			owner.agent.speed = huntSpeed;

			if (owner.agent.isOnNavMesh)
				owner.agent.SetDestination(destination);
			// Debug.Log("findign new path:" + destination);
		}
	}
}