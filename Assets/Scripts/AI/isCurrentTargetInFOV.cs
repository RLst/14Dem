using LeMinhHuy.AI.Core;
using LeMinhHuy.Character;
using UnityEngine;

namespace LeMinhHuy.AI
{
	public class isCurrentTargetInFOV : Condition
	{
		public isCurrentTargetInFOV(float FOVrange = 30f, int obstaclesLayer = 1 << 0, float eyeHeight = 1.7f)
		{
			this.sqrFOVrange = FOVrange * FOVrange;
			this.eyeHeight = eyeHeight;
			this.obstaclesLayer = obstaclesLayer;
		}
		readonly float sqrFOVrange;
		readonly float eyeHeight;
		readonly LayerMask obstaclesLayer;
		Unit currentTarget;

		public override NodeState OnExecute()
		{
			state = NodeState.Success;

			currentTarget = owner.GetData("currentTarget") as Unit;
			if (currentTarget is null) return state;

			//Check for obstacles
			if (Physics.Raycast(transform.position, currentTarget.transform.position + Vector3.up * eyeHeight * 0.5f - transform.position + Vector3.up * eyeHeight, sqrFOVrange, obstaclesLayer))
			{
				//Hit an obstacle, failed
				return state = NodeState.Failure;
			}

			//Check if in FOV
			if (Vector3.SqrMagnitude(currentTarget.transform.position - transform.position) > sqrFOVrange)
			{
				//out of range
				return state = NodeState.Failure;
			}

			//Current target is in range and in view, success
			return state;
		}
	}
}