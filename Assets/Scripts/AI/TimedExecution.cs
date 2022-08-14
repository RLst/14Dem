using UnityEngine;

namespace LeMinhHuy.AI.Core
{
	public class TimedExecution : Decorator
	{
		public TimedExecution(float delay, Node child) : base(child)
		{
			this.delay = delay;
		}

		float delay = 2f;
		float countdown = 0;

		public override NodeState OnExecute()
		{
			if (countdown <= 0)
			{
				countdown = delay;
			}

			countdown -= Time.deltaTime;

			if (countdown > 0)
			{
				//Keep timing and running the child
				child.OnExecute();
				return NodeState.Pending;
			}
			else
			{
				//Timer done! Run child Auto resets itself
				return NodeState.Success;
			}
		}
	}
}
