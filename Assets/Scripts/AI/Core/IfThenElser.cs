using System.Collections.Generic;
using UnityEngine;

namespace LeMinhHuy.AI.Core
{
	public class IfThenElser : Composite
	{
		public IfThenElser(params Node[] children) : base(children) { }

		public override NodeState OnExecute()
		{
			if (children.Length != 2 && children.Length != 3)
			{
				Debug.LogError("IfThenElseNode must have either 2 or 3 children");
				return NodeState.Failure;
			}

			int childIndex = 0;
			state = NodeState.Pending;

			if (childIndex == 0)
			{
				var childStatuses = children[0].OnExecute();

				if (childStatuses == NodeState.Pending)
				{
					return childStatuses;
				}
				else if (childStatuses == NodeState.Success)
				{
					childIndex = 1;
				}
				else if (childStatuses == NodeState.Failure)
				{
					if (children.Length == 3)
					{
						childIndex = 2;
					}
					else
					{
						return childStatuses;
					}
				}
			}

			// not an else
			if (childIndex > 0)
			{
				NodeState childStatuses = children[childIndex].OnExecute();
				if (childStatuses == NodeState.Pending)
				{
					return NodeState.Pending;
				}
				else
				{
					// haltChildren();
					childIndex = 0;
					return childStatuses;
				}
			}

			throw new System.ArithmeticException("Something unexpected happened in IfThenElseNode");
		}
	}
}