using System;

namespace LeMinhHuy.AI
{
	public class Sequence : Composite
	{
		public override NodeState OnExecute()
		{
            state = NodeState.Success;
            bool anyChildPending = false;

            foreach (var c in children)
            {
                NodeState result = c.OnExecute();
				switch (result)
				{
					//Break out and also fail the remaining children
					case NodeState.Failure:
						state = NodeState.Failure;
						continue;
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

            //Return pending if any child was pending and now failures encountered
			if (anyChildPending && state != NodeState.Failure)
				state = NodeState.Pending;

			return state;
		}
	}
}