using LeMinhHuy.Character;
using UnityEngine;

namespace LeMinhHuy.AI.Core
{
	public class Chase : Action
	{
		public Chase(float speed = 6f)
		{
			this.chaseSpeed = speed;
		}

		Unit currentTarget;
		float chaseSpeed;

		public override NodeState OnExecute()
		{
			state = NodeState.Failure;

			//get current target
			currentTarget = owner.GetData("currentTarget") as Unit;
			if (currentTarget is null)
			{
				// Debug.Log(owner.name + ": new target set!");
				return state;
			}

			//Chase after current target
			owner.agent.SetDestination(currentTarget.transform.position);
			owner.agent.speed = chaseSpeed;
			Debug.Log(owner.name + ": Chasing");
			return state = NodeState.Success;
		}
	}
}