using System;
using LeMinhHuy.AI.Core;

namespace LeMinhHuy.AI
{
	public class AggressiveBotAgent : BotAgent
	{
		protected override Node SetupTree()
		{
			var hunt = new Hunt(0.5f, 3f, 10);

			var sel = new ActiveSelector(hunt);

			var root = sel;
			return root;
		}
	}
}