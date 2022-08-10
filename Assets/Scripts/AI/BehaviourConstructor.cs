using UnityEngine;

namespace LeMinhHuy.AI
{
	[RequireComponent(typeof(BehaviourAgent))]
	public abstract class BehaviourConstructor : MonoBehaviour
	{
		//Extra data?
		UnitTeam unitTeam;

		//Core data
		BehaviourAgent agent;

		void Awake()
		{
			agent = GetComponent<BehaviourAgent>();
		}

		public abstract void Construct(BehaviourAgent agent);
	}
}
