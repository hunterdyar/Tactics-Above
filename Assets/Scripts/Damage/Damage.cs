using System.Collections;
using System.Collections.Generic;
using Attacks;
using BTween;
using Tactics;
using Tactics.Turns;
using UnityEngine;

namespace Tactics.DamageSystem
{
	//Convenience Wrapper class. I think it will end up making sense to make this a mono-behaviour that also handles animation of damage taking place.
	public class Damage
	{
		/// <summary>
		/// Deals damage and returns any animations to execute to display it.
		/// </summary>
		/// <param name="targetNode"></param>
		/// <param name="damageDescription"></param>
		/// <returns></returns>
		public static void DealDamageToNode(NavNode targetNode, Attack attack, ref Playback.Playback damagePlayback)
		{
			foreach (var entity in targetNode.NavMap.GetAllEntitiesOnNode(targetNode))
			{
				//Flash Node with color animation. Maybe Damage.
				//Replace this with a ... Action that the Node broadcasts?
				if (entity.TryGetComponent<ITakeDamage>(out var target))
				{
					target.TakeDamage(attack.Damage, ref damagePlayback);
				}
			}
		}
	}
}