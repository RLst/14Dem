using LeMinhHuy.AI.Core;
using LeMinhHuy.Character;
using UnityEngine;

namespace LeMinhHuy.AI
{
	public class hasCurrentTarget : Condition
	{
		Unit currentTarget;

		public override NodeState OnExecute()
		{
			currentTarget = owner.GetData("currentTarget") as Unit;
			Debug.Log(owner.name + ": current target = " + currentTarget);
			return (currentTarget == null) ? NodeState.Failure : NodeState.Success;
		}
	}
}