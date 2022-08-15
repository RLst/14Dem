using LeMinhHuy.AI.Core;
using LeMinhHuy.Character;
using UnityEngine;

namespace LeMinhHuy.AI
{
	public class ifNewTargetInShootingRangeSetAsCurrentTargetWithDelay : Action
	{
		public ifNewTargetInShootingRangeSetAsCurrentTargetWithDelay(float delay = 1f, float shootingRange = 15f)
		{
			this.delay = delay;
			this.sqrShootingRange = shootingRange * shootingRange;
		}

		float delay;
		float sqrShootingRange;
		float pending = 0;
		Unit newTarget;

		public override NodeState OnExecute()
		{
			state = NodeState.Failure;
			if (pending <= 0)
			{
				//Get new target
				newTarget = owner.GetData("newTarget") as Unit;
				if (newTarget is null)
				{
					// Debug.Log(owner.name + ": no new targets found");
					return NodeState.Failure;
				}

				//Check if in shooting range
				if (Vector3.SqrMagnitude(newTarget.transform.position - transform.position) < sqrShootingRange)
				{
					// Debug.Log(owner.name + ": target within shooting range");
					//Set as current target after a delay
					pending = delay;
					state = NodeState.Pending;
				}
			}
			else
			{
				//Keep delaying until you can set the new target
				pending -= Time.deltaTime;
				if (pending <= 0)
				{
					//delay done! set current target
					owner.SetData("currentTarget", newTarget);
					// Debug.Log(owner.name + ": delay done. setting new target");
					state = NodeState.Pending;
				}
			}
			return state;
		}
	}
}