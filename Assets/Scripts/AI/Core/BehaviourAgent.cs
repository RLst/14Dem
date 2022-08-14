using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace LeMinhHuy.AI.Core
{
	public abstract class BehaviourAgent : MonoBehaviour
	{
		[SerializeField] bool active = true;
		[Tooltip("Repeat entry node evaluation")]
		[SerializeField] bool repeatExecution = true;

		//Properties
		public NodeState? agentState { get; private set; }
		public bool hasAgent => agent != null;
		public NavMeshAgent agent { get; private set; }

		//Members
		Node entry;
		protected abstract Node SetupTree();

		//Local Blackboard
		private Dictionary<string, object> localBlackboard = new Dictionary<string, object>();
		public void SetData(string key, object value) => localBlackboard[key] = value;
		public object GetData(string key)
		{
			object value = null;
			if (localBlackboard.TryGetValue(key, out value))
				return value;
			return value;
		}
		public bool ClearData(string key)
		{
			if (localBlackboard.ContainsKey(key))
			{
				localBlackboard.Remove(key);
				return true;
			}
			return false;
		}

		void Awake()
		{
			agent = GetComponent<NavMeshAgent>();

			//Force tree setup
			entry = SetupTree();

			//Set owner of all nodes dribble down
			entry?.SetOwner(this);

			entry?.OnAwaken();
		}

		void Start()
		{
			entry?.OnInitiate();

			//Runs forever and doesn't need to be stopped; all coroutines will be stopped automatically if object disabled
			StartCoroutine(FixedTickRoutine());
		}

		//Custom rate tick; can help reduce cpu usage
		protected virtual IEnumerator FixedTickRoutine()
		{
			while (true)
			{
				//Must be in fixed tick mode and agent active
				if (BehaviourDirector.current.tickMode == TickMode.Fixed && active)
				{
					Tick();

					//Deactivate agent if set to not restart when evaluated
					if (!repeatExecution)
						gameObject.SetActive(false);   //_agent = false; stacks and overlaps coroutines

					yield return new WaitForSeconds(BehaviourDirector.current.tickRateSeconds);
				}
				else
				{
					//Not fixed mode
					yield return null;
				}
			}
		}

		void Update()
		{
			//Do not tick normally if set to either Fixed or Manual tick mode
			if (BehaviourDirector.current.tickMode != TickMode.Normal || !active)
				return;

			Tick();

			if (!repeatExecution)
				gameObject.SetActive(false);
		}

		/// <summary>
		/// Main tick evaluation
		/// </summary>
		internal void Tick()
		{
			// print("tick");
			entry?.OnExecute();
		}

		void OnDestroy()
		{
			entry?.OnShutdown();
		}
	}
}