using System;
using System.Collections.Generic;

namespace LeMinhHuy.AI.Core
{
	public class ActiveSelector : Composite
	{
		public ActiveSelector(params Node[] children) : base(children) { }

		//The point is to allow all pending children to run

		//If a child success, return success, finish
		//If a child fails, go to next child
		//If a child pending, go to next child
		//If at least one child pending, return pending
		//If all children return pending, return fail
		public override NodeState OnExecute()
		{
			state = NodeState.Failure;
			bool anyChildPending = false;
			foreach (var c in children)
			{
				NodeState result = c.OnExecute();
				switch (result)
				{
					case NodeState.Pending:
						anyChildPending = true;
						continue;

					case NodeState.Success:
						state = result;
						break;

					case NodeState.Failure:
						continue;

					//if failure then continue onto next child...
					default:
						throw new NotImplementedException("NodeState not implemented yet");
				}
			}
			if (anyChildPending)
				state = NodeState.Pending;
			return state;
		}
	}
}