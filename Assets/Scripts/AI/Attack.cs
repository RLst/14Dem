using LeMinhHuy.AI.Core;
using LeMinhHuy.Character;

namespace LeMinhHuy.AI
{
	public class Attack : Action
	{
		public Attack(WeaponController unitWeaponController)
		{
			this.weaponController = unitWeaponController;
		}
		WeaponController weaponController;
		public override NodeState OnExecute()
		{
			weaponController.FireWeapon();
			return state = NodeState.Success;
		}
	}
}