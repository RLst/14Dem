using UnityEngine;

namespace LeMinhHuy.AI.Core
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

		//Properties
		public BehaviourAgent owner { get; private set; }
		//Set the owner of this node and it's children
		public virtual void SetOwner(BehaviourAgent owner) => this.owner = owner;

		//Shortcuts
		protected GameObject gameObject => owner.gameObject;
		protected Transform transform => owner.transform;
		// protected Dictionary<Type, object> localBlackboard => owner.localBlackboard;
		// protected Dictionary<Type, object> globalBlackboard => BehaviourDirector.globalBlackboard;

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