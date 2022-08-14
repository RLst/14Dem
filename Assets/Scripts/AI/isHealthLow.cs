using LeMinhHuy.AI.Core;
using LeMinhHuy.Character;

namespace LeMinhHuy.AI
{
	public class isHealthLow : Condition
	{
		public isHealthLow(Unit unit, float lowHealthThreshold = 20f)
		{
			this.unit = unit;
			this.lowHealthThreshold = lowHealthThreshold;
		}

		Unit unit;
		float lowHealthThreshold;

		public override NodeState OnExecute()
		{
			state = NodeState.Failure;
			if (unit.health < lowHealthThreshold)
			{
				state = NodeState.Success;
			}
			return state;
		}
	}
}