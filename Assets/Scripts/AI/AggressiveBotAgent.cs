using System;
using LeMinhHuy.AI.Core;
using LeMinhHuy.Character;
using UnityEngine;

namespace LeMinhHuy.AI
{
	public class AggressiveBotAgent : BotAgent
	{
		[SerializeField] float huntSpeed = 2.5f;
		[SerializeField] float huntArriveDist = 0.5f;
		[SerializeField] float huntDist = 10f;
		[SerializeField] float chaseSpeed = 6f;
		[SerializeField] float retreatSpeed = 4f;
		[SerializeField] float FOVrange = 30f;
		[SerializeField] float shootRange = 10f;
		[SerializeField] LayerMask obstaclesLayer = 1 << 0;

		[SerializeField] WeaponController weaponController;

		protected override Node SetupTree()
		{

			var hunt = new Hunt(0.5f, 3f, 10);

			var reload = new Reload(weaponController);
			var enoughAmmo = new hasEnoughAmmo(weaponController);
			var notEnoughAmmo = new Inverter(enoughAmmo);
			var ifNotEnoughAmmoThenReload = new Sequence(notEnoughAmmo, reload);

			var shoot = new Attack(weaponController);
			var ifCurrentTargetInFOV = new isCurrentTargetInFOV(FOVrange, obstaclesLayer);
			var chase = new Chase(chaseSpeed);
			var hasCurrentTarget = new hasCurrentTarget();
			var ifCurrentTargetInRangeAndInViewThenChase = new Sequence()

			var chaseReloadOrAttack = new Sequence();

			var

			//Root
			return new ActiveSelector(hunt);
		}
	}
}