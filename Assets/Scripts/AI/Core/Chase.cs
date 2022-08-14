using LeMinhHuy.Character;

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
			if (currentTarget is null) return state;

			//Chase after current target
			owner.agent.SetDestination(currentTarget.transform.position);
			owner.agent.speed = chaseSpeed;
			return state = NodeState.Success;
		}
	}
}