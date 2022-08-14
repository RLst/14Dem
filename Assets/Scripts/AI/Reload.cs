using LeMinhHuy.AI.Core;
using LeMinhHuy.Character;

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
			return state = NodeState.Success;
		}
	}
}