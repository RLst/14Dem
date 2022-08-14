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
		[SerializeField] float FOVrange = 30f;
		[SerializeField] float retreatSpeed = 4f;
		[SerializeField] float retreatDetectionRange = 30f;
		[SerializeField] float shootRange = 10f;
		[SerializeField] float changeTargetDelay = 1f;


		[SerializeField] LayerMask obstaclesLayer = 1 << 0;
		[SerializeField] LayerMask unitLayer = 1 << 6;

		[SerializeField] WeaponController weaponController;

		protected override Node SetupTree()
		{

			var HUNT = new Hunt(0.5f, 3f, 10);

			//Chase, reload and attacks
				var A_Chase = new Chase(chaseSpeed);
					var A_Shoot = new Attack(weaponController);
						var A_Reload = new Reload(weaponController);
							var C_enoughAmmo = new hasEnoughAmmo(weaponController);
						var I_notEnoughAmmo = new Inverter(C_enoughAmmo);
					var SEQ4 = new Sequence(I_notEnoughAmmo, A_Reload);
					var C_ifCurrentTargetInFOV = new isCurrentTargetInFOV(FOVrange, obstaclesLayer);
				var SEQ3 = new Sequence(C_ifCurrentTargetInFOV, SEQ4, A_Shoot);
				var C_hasCurrentTarget = new hasCurrentTarget();
			var CHASE_RELOAD_ATTACK = new Sequence(C_hasCurrentTarget, SEQ3, A_Chase);

			//Targeting
					var A_ifNewTargetInShootingRangeSetAsCurrentTargetWithDelay = new ifNewTargetInShootingRangeSetAsCurrentTargetWithDelay(changeTargetDelay, shootRange);
						var A_setNewTargetAsCurrentTarget = new SetNewTargetAsCurrentTarget();
						var I_noCurrentTarget = new Inverter(C_hasCurrentTarget);
					var SEQ2 = new Sequence(I_noCurrentTarget, A_setNewTargetAsCurrentTarget);
				var SEL1 = new Selector(SEQ2, A_ifNewTargetInShootingRangeSetAsCurrentTargetWithDelay);
				var C_isNewTargetInFOV = new isNewTargetInFOV(enemies, FOVrange);
			var TARGETING = new Sequence(C_isNewTargetInFOV, SEL1);

			//Retreat
				var A_retreat = new Retreat(allies, retreatDetectionRange, unitLayer);
				var C_isHealthLow = new isHealthLow(unit);
			var RETREAT = new Sequence(C_isHealthLow, A_retreat);

			//Root
			return new ActiveSelector(RETREAT, TARGETING, CHASE_RELOAD_ATTACK, HUNT);
		}
	}
}