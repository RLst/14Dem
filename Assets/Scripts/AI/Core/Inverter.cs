namespace LeMinhHuy.AI.Core
{
	public class Inverter : Decorator
	{
		public Inverter(Node child) : base(child)
		{
		}

		public override NodeState OnExecute()
		{
			var result = child.OnExecute();
			switch (result)
			{
				case NodeState.Success: return NodeState.Failure;
				case NodeState.Failure: return NodeState.Success;
			}
			return result;  //If Pending; can't invert
		}
	}
}