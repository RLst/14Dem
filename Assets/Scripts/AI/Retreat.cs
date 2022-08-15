using System.Collections.Generic;
using LeMinhHuy.AI.Core;
using LeMinhHuy.Character;
using UnityEngine;

namespace LeMinhHuy.AI
{
	public class Retreat : Action
	{
		public Retreat(Unit[] allies, float detectionRadius, int unitLayer = 1 << 6)
		{
			this.allies = allies;
			this.sqrDetectionRadius = detectionRadius * detectionRadius;
			this.unitLayer = unitLayer;
		}

		Unit[] allies;
		float sqrDetectionRadius;       //Improve performance
		LayerMask unitLayer = 1 << 6;

		public override NodeState OnExecute()
		{
			//Not pending as I don't want active selector to continue onto next branch
			//Find allies within range
			List<Unit> alliesInRange = new List<Unit>();
			foreach (var a in allies)
			{
				if (Vector3.SqrMagnitude(a.transform.position - transform.position) < sqrDetectionRadius)
				{
					alliesInRange.Add(a);
				}
			}

			if (alliesInRange.Count == 0) return NodeState.Failure;

			//Calculate the average of the team
			//Add all positions / number of allies
			Vector3 avgPos = Vector3.zero;
			foreach (var a in alliesInRange)
			{
				avgPos += a.transform.position;
			}
			avgPos /= alliesInRange.Count;

			//Move towards them
			owner.agent.SetDestination(avgPos);

			// Debug.Log(owner.name + ": Retreating");
			return NodeState.Success;
		}
	}
}