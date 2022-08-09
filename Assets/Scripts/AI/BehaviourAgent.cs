using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeMinhHuy.AI
{
	public class BehaviourAgent : MonoBehaviour
	{
		[SerializeField] bool active = true;
		[Tooltip("Repeat entry node evaluation")]
		[SerializeField] bool repeatExecution = true;

		//Properties
		public NodeState? agentState { get; private set; }

		//Members
		Node entry;
		public void SetEntryNode(Node entry) => this.entry = entry;     //Set the AI of this agent externally

		void Awake()
		{
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

					yield return new WaitForSeconds(BehaviourDirector.current.tickRate);
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
			entry?.OnExecute();
		}

		void OnDestroy()
		{
			entry?.OnShutdown();
		}
	}
}