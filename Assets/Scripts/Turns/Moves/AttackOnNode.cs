using System.Collections;
using Attacks;
using Tactics.DamageSystem;
using Tactics.Entities;
using UnityEngine;

namespace Tactics.Turns
{
	public class AttackOnNode : MoveBase
	{
		private NavNode _node;
		private Attack _attack;

		public AttackOnNode(Agent agent, NavNode node, Attack attack) : base(agent)
		{
			_node = node;
			_attack = attack;
		}

		public override IEnumerator DoMove()
		{
			Playback.Playback movePlayback = new Playback.Playback();
			Debug.Log(_agent.name + " is attacking with " + _attack.name);

			Damage.DealDamageToNode(_node, _attack, ref movePlayback);
			

			movePlayback.Start();
			while (movePlayback.IsRunning())
			{
				yield return null;
			}

		}
	}
}