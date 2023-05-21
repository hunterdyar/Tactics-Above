using System.Collections;
using System.Collections.Generic;
using Attacks;
using Tactics.DamageSystem;
using Tactics.Entities;
using UnityEngine;

namespace Tactics.Turns
{
	public class AttackOnNodes : MoveBase
	{
		private List<NavNode> _nodes;
		private Attack _attack;

		public AttackOnNodes(Agent agent, List<NavNode> nodes, Attack attack) : base(agent)
		{
			_nodes = nodes;
			_attack = attack;
		}

		public override IEnumerator DoMove()
		{
			Playback.Playback movePlayback = new Playback.Playback();
			Debug.Log(_agent.name + " is attacking " + _nodes.Count + " nodes with " + _attack.name);

			foreach (var node in _nodes)
			{
				Damage.DealDamageToNode(node, _attack, ref movePlayback);
			}

			movePlayback.Start();
			while (movePlayback.IsRunning())
			{
				yield return null;
			}

		}
	}
}