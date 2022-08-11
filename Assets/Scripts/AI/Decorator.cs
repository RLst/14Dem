namespace LeMinhHuy.AI
{
	public abstract class Decorator : Node
	{
		public Node child = null;
		public override bool hasChild => child is object;
		public override void SetOwner(BehaviourAgent owner)
		{
			this.SetOwner(owner);
			child.SetOwner(owner);
		}
		public override void OnAwaken() => child?.OnAwaken();
		public override void OnInitiate() => child?.OnInitiate();
		public override void OnShutdown() => child?.OnShutdown();
	}
}