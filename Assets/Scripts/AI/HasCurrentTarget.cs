using LeMinhHuy.AI.Core;
using LeMinhHuy.Character;

namespace LeMinhHuy.AI
{
	public class hasCurrentTarget : Condition
	{
		Unit currentTarget;

		public override NodeState OnExecute()
		{
			currentTarget = owner.GetData("currentTarget") as Unit;
			return (currentTarget == null) ? NodeState.Failure : NodeState.Success;
		}
	}
}