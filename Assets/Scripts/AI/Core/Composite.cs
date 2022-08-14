using System.Collections.Generic;
using System.Linq;

namespace LeMinhHuy.AI.Core
{
	public abstract class Composite : Node
	{
		public Composite(params Node[] children)
		{
			this.children = children;
		}

		public Node[] children;
		public override bool hasChild => children.Length > 0;

		//Set owner for this node and all its children
		public override void SetOwner(BehaviourAgent owner)
		{
			this.owner = owner;
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