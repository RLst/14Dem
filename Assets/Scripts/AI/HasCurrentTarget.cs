using LeMinhHuy.AI.Core;
using LeMinhHuy.Character;

namespace LeMinhHuy.AI
{
	public class HasCurrentTarget : Condition
	{
		//Returns true if owner has a current enemy target
		// Unit target;
		public override NodeState OnExecute()
		{
			var currentTarget = owner.GetData("currentTarget") as Unit;
			return currentTarget is null ? NodeState.Failure : NodeState.Success;
		}
	}
}