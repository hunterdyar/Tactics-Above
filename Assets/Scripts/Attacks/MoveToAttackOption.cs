using System;
using System.Collections.Generic;
using Tactics;
using Tactics.Entities;
using UnityEngine;

namespace Attacks
{
	public struct MoveToAttackOption
	{
		public Attack Attack;
		public NavNode[] TargetNode;
		public Agent[] targets;
		public NavNode destination;
		public Vector3Int facingDirection;
		public int stepsStillToMove;

		public MoveToAttackOption(Attack attack, NavNode center, NavNode targetNode, Vector3Int facing, int steps=0)
		{
			Attack = attack;
			destination = center;
			TargetNode = new []{targetNode};
			facingDirection = facing;
			targets = Array.Empty<Agent>();
			stepsStillToMove = steps;
		}

		public MoveToAttackOption(Attack attack, NavNode center, List<NavNode> targetNodes, Vector3Int facing, int steps=0)
		{
			Attack = attack;
			destination = center;
			TargetNode = targetNodes.ToArray();
			facingDirection = facing;
			targets = Array.Empty<Agent>();
			stepsStillToMove = steps;
		}
		public int GetSimulatedDamageDealt()
		{
			if (targets.Length == 0)
			{
				return 0;
			}
			//todo simulate this with the agent to get it's strengths/weaknesses to.
			return Attack.Damage.Amount;
		}

		public bool TryFindTargets(Faction enemies)
		{
			List<Agent> agentTargets = new List<Agent>();
			foreach (var node in TargetNode)
			{
				if (enemies.TryGetEntity(node, out var enemy))
				{
					if (enemy is Agent enemyAgent)
					{
						agentTargets.Add(enemyAgent);
					}
					else
					{
						//do we care about hitting ... walls? destroying traps?
					}
				}
			}

			if (agentTargets.Count > 0)
			{
				targets = agentTargets.ToArray();
				return true;
			}

			return false;
		}
	}
}