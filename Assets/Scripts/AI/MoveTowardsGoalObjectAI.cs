using System.Collections.Generic;
using System.Linq;
using Attacks;
using Tactics.DamageSystem;
using Tactics.Entities;
using Tactics.GridShapes;
using Tactics.Pathfinding;
using Tactics.Turns;
using Tactics.Utility;
using UnityEngine;

namespace Tactics.AI
{
	public class MoveTowardsGoalObjectAI : MoveDecider
	{
		[SerializeField] private int movesPerTurn;
		private AStarPathfinder<NavNode> _pathfinder;
		[SerializeField] private Attack _attack;
		public override void Initiate(Agent agent)
		{
			base.Initiate(agent);
			_pathfinder = new AStarPathfinder<NavNode>(agent.CurrentNode.NavMap, agent.IsNodeWalkable);
		}

		public override MoveBase DecideMove(AIContext context)
		{
			//If we are able to attack an enemy, attack an enemy.
			foreach (var attack in _attack.GetAIActions(_agent))
			{
				foreach (var node in attack.TargetNodes)
				{
					if (_agent.EnemyLayer.HasAnyEntity(node))
					{
						//We will hit anythng!  Let's attack!
						return attack.GetMove();
					}
				}
				
			}//else....

			//Move towards the closest enemy. It doesn't consider pathfinding right now.
			if(_agent.EnemyLayer.TryGetClosestAgentInMap(CurrentNode,out var target))
			{
				//todo: an easy way to try to get "close" to a target, or stop some distance away.
				//What we ACTUALLY want to do is find all of the positions around an enemy we could be that will attack them, and then move towards the closest one.
				//So after finding the closest enemy, we want to find the best attack position.
				if (TryGetClosestAttackingPosition(target.CurrentNode, CurrentNode, out var attackingPos))
				{
					
					//If we put enemy layer on the agent's no-walking-here list, which we should do, it will break the current pathfinder - never able to find the target.
					_pathfinder.TryFindPath(CurrentNode, attackingPos, out var pathList);

					//no valid path
					if (pathList.Count == 0)
					{
						return new DoNothingMove(_agent);
					}

					//valid path...
					pathList.RemoveAt(pathList.Count - 1); //remove last item so we don't step onto enemy, but stay one away.
					if (pathList.Count == 0)
					{
						//We don't want to step on the enemy.
						//todo: have to determine how to move towards-but-not-on. goal is NEAR an enemy.
						return new DoNothingMove(_agent);
					}

					return new MoveAlongPath(_agent, pathList, movesPerTurn);
				}
			}
			
			//could not move towards closest enemy, we do not fall back to next closest.
			return new DoNothingMove(_agent);
		}

		//todo: could be static
		private bool TryGetClosestAttackingPosition(NavNode targetNode, NavNode currentNode, out NavNode closestAttackPos)
		{
			List<NavNode> attackingPositions = new List<NavNode>();
			foreach (var delta in _attack.Shape.Shape)
			{
				var attackPos = targetNode.GridPosition - new Vector3Int(delta.x, 0, delta.y);
				if (targetNode.NavMap.TryGetNavNode(attackPos, out var node))
				{
					if (_agent.IsNodeWalkable(node))
					{
						attackingPositions.Add(node);
					}
				}
			}
			if (attackingPositions.Count > 0)
			{
				closestAttackPos = attackingPositions.OrderBy(x => Vector3Int.Distance(currentNode.GridPosition, x.GridPosition)).First();
				return true;
			}

			closestAttackPos = null;
			return false;
		}
		
	}
}