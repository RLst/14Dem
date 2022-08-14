using LeMinhHuy.AI.Core;
using LeMinhHuy.Character;

namespace LeMinhHuy.AI
{
	public class hasEnoughAmmo : Condition
	{
		WeaponController weaponController;

		public hasEnoughAmmo(WeaponController unitWeaponController)
		{
			this.weaponController = unitWeaponController;
		}

		public override NodeState OnExecute()
		{
			state = NodeState.Failure;
			if (weaponController.currentWeaponHasAmmo)
				return state = NodeState.Success;
			return state;
		}
	}
}