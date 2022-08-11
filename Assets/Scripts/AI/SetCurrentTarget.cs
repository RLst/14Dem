using UnityEngine;

namespace LeMinhHuy.AI
{
	public class SetCurrentTarget : Action
	{
		public override NodeState OnExecute()
		{
			var currentTarget = owner.GetData("currentTarget") as Unit;
			if (currentTarget is null)
			{
				Debug.Log("No current target found!");
				return NodeState.Failure;
			}
			// var target = owner.GetData("currentTarget")
			throw new System.NotImplementedException();
		}
	}
}