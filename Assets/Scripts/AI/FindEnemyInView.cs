using LeMinhHuy.AI.Core;
using LeMinhHuy.Character;

namespace LeMinhHuy.AI
{
	public class FindEnemyInView : Condition
	{
		public FindEnemyInView(float FOVrange = 30f)
		{
			this.sqrFOVrange = FOVrange * FOVrange;
		}

		float sqrFOVrange;
		BotAgent botOwner;
		Unit newTarget;

		public override void OnAwaken()
		{
			base.OnAwaken();
			botOwner = owner as BotAgent;
		}

		public override NodeState OnExecute()
		{
			//Loop through all enemies and find the closest one
			float closestSqrDist = float.PositiveInfinity;
			newTarget = null;
			foreach (var e in botOwner.enemies)
			{
				var sqrDist = (e.transform.position - owner.transform.position).sqrMagnitude;
				if (sqrDist < closestSqrDist)
				{
					closestSqrDist = sqrDist;
					newTarget = e;
				}
			}
			if (newTarget is null)
				return NodeState.Failure;

			//If there is one then save to local blackboard
			owner.SetData("newTarget", newTarget);
			return NodeState.Success;
		}
	}
}