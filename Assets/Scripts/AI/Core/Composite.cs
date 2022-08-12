using System.Collections.Generic;

namespace LeMinhHuy.AI.Core
{
	public abstract class Composite : Node
	{
		public List<Node> children = new List<Node>();
		public override bool hasChild => children.Count > 0;

		//Set owner for this node and all its children
		public override void SetOwner(BehaviourAgent owner)
		{
			SetOwner(owner);
			foreach (var c in children)
				c.SetOwner(owner);
		}

		public override void OnAwaken()
		{
			foreach (var c in children)
				c.OnAwaken();
		}
		public override void OnInitiate()
		{
			foreach (var c in children)
				c.OnInitiate();
		}
		public override void OnShutdown()
		{
			foreach (var c in children)
				c.OnShutdown();
		}
	}
}