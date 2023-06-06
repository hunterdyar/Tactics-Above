using Tactics.AI.InfluenceMaps;
using Tactics.Entities;
using Tactics.Pathfinding;
using Tactics.Turns;
using Tactics.Utility;
using UnityEngine;

namespace Tactics.AI.Actions
{
	[CreateAssetMenu(fileName = "Move To Frontlines Acton", menuName = "Tactics/Actions/Move To Frontlines", order = 0)]
	public class MoveToFrontlines : ScriptableAction
	{
	
		private NavNode _targetNode;
		[SerializeField] private int movementTurnsToConsider;
		public override float ScoreAction(Agent agent, AIContext context)
		{
			//Finds the highest point in current movement range that is in enemy territory.

			//TODO: find lowest point in territory in our movement range.
			var map = InfluenceMap.Clone(context.BattleMap);
			map.MultiplyInfluence(agent.GetMovementRangeMap(movementTurnsToConsider));
			var lowest = map.GetLowestPosition();
			_targetNode = agent.CurrentNode.NavMap.GetNavNode(lowest.V2ToV3XZ());
			_agent = agent;
			//Action Veto: If there is no lowest point in our movement range, we can't move into enemy territory.
			if (map.GetValue(lowest) >= 0)
			{
				Score = 0;
				return 0;
			}

			return base.ScoreAction(agent, context);
		}

		public override MoveBase GetMove()
		{
			//todo: get moves from agent by passing in target destination.
			var pathfinder = new AStarPathfinder<NavNode>(_agent.CurrentNode.NavMap);
			if (_targetNode == null)
			{
				return new DoNothingMove(_agent);
			}

			pathfinder.TryFindPath(_agent.CurrentNode, _targetNode, out var pathList);

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

			return new MoveAlongPath(_agent, pathList, _agent.range);
		}
		
	}
}