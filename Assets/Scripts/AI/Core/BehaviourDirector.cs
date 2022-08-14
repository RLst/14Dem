using LeMinhHuy.Utils;

namespace LeMinhHuy.AI.Core
{
	public class BehaviourDirector : Singleton<BehaviourDirector>
	{
		//Inspector
		public TickMode tickMode;
		public float tickRateFPS = 30;
		public float tickRateSeconds => 1 / tickRateFPS;

		//Members
		BehaviourAgent[] allAgents;

		void Awake() => allAgents = FindObjectsOfType<BehaviourAgent>();

		//Manually tick all agents
		public void ManualTick()
		{
			foreach (var a in allAgents)
				a.Tick();
		}

		//Tick a specific agent
		public void TickAgent(BehaviourAgent agent)
		{
			agent.Tick();
		}
	}
}