using LeMinhHuy.Utils;
using UnityEngine;

namespace LeMinhHuy.AI
{
	public class BehaviourDirector : Singleton<BehaviourDirector>
	{
		//Inspector
		public TickMode tickMode { get; set; }
		public float tickRate;

		//Members
		BehaviourAgent[] allAgents;

		void Awake() => allAgents = FindObjectsOfType<BehaviourAgent>();

		public void ManualTick()
		{
			foreach (var a in allAgents)
				a.Tick();
		}

		public void TickAgent(BehaviourAgent agent)
		{
			agent.Tick();
		}
	}
}