using System;
using System.Collections.Generic;

namespace LeMinhHuy.AI.Core
{
	public class ActiveSequence : Composite
	{
		public ActiveSequence(params Node[] children) : base(children) { }

		//The point is to give the best chance for all pending children to run

		//If child success, go to next child
		//If child fails, return fail, end
		//If child pending, go to next child
		//If at least one child pending, return pending
		//If all children return pending, return success
		public override NodeState OnExecute()
		{
			state = NodeState.Success;
			bool anyPendingChildren = false;
			foreach (var c in children)
			{
				NodeState result = c.OnExecute();
				switch (result)
				{
					case NodeState.Pending:
						anyPendingChildren = true;
						continue;

					case NodeState.Failure:
						state = result;
						break;

					//if success then continue onto next child...
					default:
						throw new NotImplementedException("NodeState not implemented yet");
				}
			}
			if (anyPendingChildren)
				state = NodeState.Pending;
			return state;
		}
	}
}
//Return pending if at least ONE child returns pending
// state = anyChildPending ? NodeState.Pending : NodeState.Success;