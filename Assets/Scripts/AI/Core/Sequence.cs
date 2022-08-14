using System;
using System.Collections.Generic;

namespace LeMinhHuy.AI.Core
{
	public class Sequence : Composite
	{
		public Sequence(params Node[] children) : base(children) { }

		//If child success, go to next child
		//If child fails, return fail, end
		//If child pending, return pending, end
		public override NodeState OnExecute()
		{
			state = NodeState.Success;
			foreach (var c in children)
			{
				NodeState result = c.OnExecute();
				switch (result)
				{
					case NodeState.Pending:
					case NodeState.Failure:
						state = result;
						break;
					//if success then continue onto next child...
					default:
						throw new NotImplementedException("NodeState not implemented yet");
				}
			}
			return state;
		}
	}
}
//Return pending if at least ONE child returns pending
// state = anyChildPending ? NodeState.Pending : NodeState.Success;