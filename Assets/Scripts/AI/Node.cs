using UnityEngine;

namespace LeMinhHuy.AI
{
	[System.Serializable]
	public abstract class Node
	{
		public Node() { }
		public Node(Node parent, BehaviourAgent owner)
		{
			this.parent = parent;
			this.owner = owner;
		}
		//Set the owner of this node and all nodes below
		public virtual void SetOwner(BehaviourAgent owner) => this.owner = owner;

		//Properties
		public BehaviourAgent owner { get; set; }
		protected GameObject gameObject => owner.gameObject;    //Shortcuts
		protected Transform transform => owner.transform;

		//Members
		public abstract bool hasChild { get; }
		protected Node parent;
		protected NodeState state;

		//Called at Awake()
		public virtual void OnAwaken() { }
		//Called at Start()
		public virtual void OnInitiate() { }
		//Called at Update() or user defined tick cycle
		public abstract NodeState OnExecute();
		//Called at OnDestroy()
		public virtual void OnShutdown() { }
	}
}