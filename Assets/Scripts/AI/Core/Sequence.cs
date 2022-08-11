using System;

namespace LeMinhHuy.AI
{
	public class Sequence : Composite
	{
		//Returns SUCCESS if all children return SUCCESS
		//If a child returns SUCCESS or PENDING then it will go to the next children
		//If a child returns FAILURE then it will halt and return FAILURE
		//If no failures but at least ONE child returned pending, then return pending
		public override NodeState OnExecute()
		{
			bool anyChildPending = false;

			foreach (var c in children)
			{
				NodeState result = c.OnExecute();
				switch (result)
				{
					//Break out and also fail the remaining children
					case NodeState.Failure:
						state = NodeState.Failure;
						return state;
					//Continue with next child if success found
					case NodeState.Success:
						continue;
					//Set child running flag and continue
					case NodeState.Pending:
						anyChildPending = true;
						continue;

					default:
						throw new NotImplementedException("NodeState not implemented yet");
				}
			}

			//Return pending if at least ONE child returns pending
			state = anyChildPending ? NodeState.Pending : NodeState.Success;
			return state;
		}
	}
}