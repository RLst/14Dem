using LeMinhHuy.AI.Core;
using LeMinhHuy.Character;
using UnityEngine;

namespace LeMinhHuy.AI
{
	public class Reload : Action
	{
		public Reload(WeaponController unitWeaponController)
		{
			this.weaponController = unitWeaponController;
		}
		WeaponController weaponController;
		public override NodeState OnExecute()
		{
			weaponController.ReloadWeapon();
			// Debug.Log(owner.name + ": reloading");
			return state = NodeState.Success;
		}
	}
}