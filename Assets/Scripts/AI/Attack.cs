using LeMinhHuy.AI.Core;
using LeMinhHuy.Character;
using UnityEngine;

namespace LeMinhHuy.AI
{
	public class Attack : Action
	{
		public Attack(WeaponController unitWeaponController, AimController unitAimController)
		{
			this.weaponController = unitWeaponController;
			this.aimController = unitAimController;
		}

		WeaponController weaponController;
		AimController aimController;

		public override NodeState OnExecute()
		{
			aimController.Aim();	//TODO: need to be able to pass in a position
			weaponController.FireWeapon();
			Debug.Log(owner.name + ": shooting");
			return state = NodeState.Success;
		}
	}
}