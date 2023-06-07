using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Tactics;
using Tactics.Entities;
using UnityEngine;

namespace Attacks
{
	public class AttackOptionCalculator : MonoBehaviour
	{
		private Agent _agent;
	
		private readonly Dictionary<NavNode, List<MoveToAttackOption>> _optionsByLocation = new Dictionary<NavNode, List<MoveToAttackOption>>();
		
		private Faction enemies => _agent.EnemyLayer;
		
		public void Init(Agent agent)
		{
			_agent = agent;
		}

		public void CalculateMovementToAttackOptions(int turns = 1)
		{
			//check cache and don't recalculate if we don't need to.
			_optionsByLocation.Clear();
			//this includes the agents current node, which is good - we will use this cached data for regular (non movement) attack options, even if I haven't refactored that part yet.
			FloodFindAttacksForNodes(_agent.CurrentNode,_agent.range);
		}

		private void FloodFindAttacksForNodes(NavNode pos, int step)
		{
			if (step <= 0)
			{
				return;
			}

			step = step - pos.WalkCost;
			if (!_optionsByLocation.ContainsKey(pos))
			{
				FindAttacksForNode(pos, _agent.range - step);
				var neighbors = pos.NavMap.GetNeighborNodes(pos, true);

				foreach (var n in neighbors)
				{
					if (step > 0 && _agent.IsNodeWalkable((NavNode)n))
					{
						FloodFindAttacksForNodes((NavNode)n, step);
					}
				}
			}
		}

		private void FindAttacksForNode(NavNode agentNode, int stepsToPos)
		{
			var attacks = _agent.Attacks;
			foreach (var attack in attacks)
			{
				//todo: make one-line
				var options = attack.GetAttackOptionsForLocation(agentNode,stepsToPos);
				
				//Cull away attacks that don't hit anything. NOTE for now we cast to agents, and won't hit grid entities like traps.
				options = options.Where(x => x.TryFindTargets(enemies)).ToList();

				if (options.Count > 0)
				{
					if (_optionsByLocation.TryGetValue(agentNode, out var existingOptions))
					{
						//more than one attack is landing hits.
						
						//append list
						existingOptions.AddRange(options);
					}
					else
					{
						_optionsByLocation.Add(agentNode,options);
					}
				}
			}
		}

		public Dictionary<NavNode, List<MoveToAttackOption>> GetOptionsByLocation()
		{
			return _optionsByLocation;
		}
	}
}