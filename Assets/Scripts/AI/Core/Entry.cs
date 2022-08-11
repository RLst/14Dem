namespace LeMinhHuy.AI
{
	//Is this really required?
	public class Entry : Node
	{
		public override bool hasChild => throw new System.NotImplementedException();

		public override NodeState OnExecute()
		{
			throw new System.NotImplementedException();
		}

		public override void SetOwner(BehaviourAgent owner)
		{
			throw new System.NotImplementedException();
		}
	}
}