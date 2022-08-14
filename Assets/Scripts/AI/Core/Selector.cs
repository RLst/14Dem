using System;
using System.Collections.Generic;

namespace LeMinhHuy.AI.Core
{
	public class Selector : Composite
	{
		public Selector(params Node[] children) : base(children) { }

		//If child success, return success, finish
		//If child fails, go to next child
		//If child pending, return pending, finish
		//If all children fails, return fail, finish
		public override NodeState OnExecute()
		{
			state = NodeState.Failure;
			foreach (var c in children)
			{
				NodeState result = c.OnExecute();
				switch (result)
				{
					case NodeState.Pending:
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
			return state;
		}
	}
}