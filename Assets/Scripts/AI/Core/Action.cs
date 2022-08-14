using LeMinhHuy.Character;

namespace LeMinhHuy.AI.Core
{
	public abstract class Action : Leaf { }
	public class Chase : Action
	{
		//if there's a current target then chase the current target
		//get the current target by 
		Unit currentTarget;



		public override NodeState OnExecute()
		{
			throw new System.NotImplementedException();
		}
	}
}