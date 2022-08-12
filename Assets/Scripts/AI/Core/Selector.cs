using System;

namespace LeMinhHuy.AI.Core
{
	public class Selector : Composite
	{
		//Returns SUCCESS if one of the children returns SUCCESS OR PENDING and not process any further nodes
		//If a child returns FAIL then it will go to the next child
		public override NodeState OnExecute()
		{
			state = NodeState.Failure;
			foreach (var c in children)
			{
				NodeState result = c.OnExecute();
				switch (result)
				{
					//Continue on with next child if failure
					case NodeState.Failure:
						continue;
					//Return success instantly if found
					case NodeState.Success:
					//Return pending only after no successes found
					case NodeState.Pending:
						state = result;
						break;

					default:
						throw new NotImplementedException("NodeState not implemented yet");
				}
			}
			return state;
		}
	}
}