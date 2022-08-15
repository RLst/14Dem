using System;
using LeMinhHuy.AI.Core;
using LeMinhHuy.Character;
using UnityEngine;

namespace LeMinhHuy.AI
{
	public class AggressiveBotAgent : BotAgent
	{
		public float huntSpeed = 2.5f;
		public float huntArriveDist = 0.5f;
		public float huntDist = 10f;
		public float chaseSpeed = 6f;
		public float FOVrange = 30f;
		public float retreatSpeed = 4f;
		public float retreatDetectionRange = 30f;
		public float shootRange = 10f;
		public float changeTargetDelay = 1f;

		public LayerMask obstaclesLayer = 1 << 0;
		public LayerMask unitLayer = 1 << 6;

		protected override Node SetupTree()
		{

			var HUNT = new Hunt(huntArriveDist, huntSpeed, huntDist);

			//Chase, reload and attacks
				var A_Chase = new Chase(chaseSpeed);
					var A_Shoot = new Attack(weaponController, aimController);
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

			//Root
			return new Selector(TARGETING, CHASE_RELOAD_ATTACK, HUNT);
			// return new ActiveSelector(TARGETING, HUNT);
		}
	}
}