using LeMinhHuy.AI.Core;
using LeMinhHuy.Character;
using UnityEngine;

namespace LeMinhHuy.AI
{
	public class SetNewTargetAsCurrentTarget : Action
	{
		Unit currentTarget;
		public override NodeState OnExecute()
		{
			var newTarget = owner.GetData("newTarget") as Unit;
			if (newTarget is null)
			{
				Debug.Log("No current target found!");
				return NodeState.Failure;
			}

			owner.SetData("currentTarget", newTarget);
			return NodeState.Success;
		}
	}
}