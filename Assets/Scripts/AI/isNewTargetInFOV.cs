using LeMinhHuy.AI.Core;
using LeMinhHuy.Character;
using UnityEngine;

namespace LeMinhHuy.AI
{
	public class isNewTargetInFOV : Condition
	{
		public isNewTargetInFOV(Unit[] enemies, float FOVrange = 30f)
		{
			this.enemies = enemies;
			this.sqrFOVrange = FOVrange * FOVrange;
		}

		float sqrFOVrange;
		// BotAgent botOwner;
		Unit newTarget;
		Unit[] enemies;

		public override NodeState OnExecute()
		{
			//Loop through all enemies and find the closest one
			float closestSqrDist = float.PositiveInfinity;
			newTarget = null;
			foreach (var e in enemies)
			{
				var sqrDist = (e.transform.position - owner.transform.position).sqrMagnitude;
				if (sqrDist < closestSqrDist)
				{
					closestSqrDist = sqrDist;
					newTarget = e;
				}
			}

			if (newTarget is null)
			{
				// Debug.Log(owner.name + ": New target not detected in FOV");
				return NodeState.Failure;
			}

			//If there is one then save to local blackboard
			owner.SetData("newTarget", newTarget);
			Debug.Log(owner.name + ": New target detected in FOV");
			return NodeState.Success;
		}
	}
}