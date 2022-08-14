using UnityEngine;

namespace LeMinhHuy
{
	public abstract class State : MonoBehaviour
	{
		[TextArea(0, 2), SerializeField] string Description = null;

		public virtual void OnEnter(UltraStateMachine sm) { }

		public virtual void OnUpdate(UltraStateMachine sm) { }

		public virtual void OnExit(UltraStateMachine sm) { }

		public virtual void OnCover(UltraStateMachine sm) { }

		public virtual void OnUncover(UltraStateMachine sm) { }
	}
}
