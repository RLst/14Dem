using UnityEngine;

namespace LeMinhHuy.AI.Core
{
	public class Delay : Action
	{
		public Delay(float delay = 2f)
		{
			this.delay = delay;
		}

		float delay;
		float countdown = -1;

		public override NodeState OnExecute()
		{
			if (countdown <= 0)
			{
				countdown = delay;
			}

			countdown -= Time.deltaTime;

			if (countdown > 0)
			{
				return NodeState.Pending;
			}
			else
			{
				return NodeState.Success;
			}
		}
	}
}


/* Nodes req

isHealthLow : Condition
Retreat : Action
isNewTargetInView : Condition
SetTarget
ClearTarget
Delay
hasTarget






*/