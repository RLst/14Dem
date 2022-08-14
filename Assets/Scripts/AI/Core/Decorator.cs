namespace LeMinhHuy.AI.Core
{
	public abstract class Decorator : Node
	{
		protected Decorator(Node child)
		{
			this.child = child;
		}

		public Node child = null;
		public override bool hasChild => child != null;
		public override void SetOwner(BehaviourAgent owner)
		{
			this.owner = owner;
			child.SetOwner(owner);
		}
		public override void OnAwaken() => child?.OnAwaken();
		public override void OnInitiate() => child?.OnInitiate();
		public override void OnShutdown() => child?.OnShutdown();
	}
}