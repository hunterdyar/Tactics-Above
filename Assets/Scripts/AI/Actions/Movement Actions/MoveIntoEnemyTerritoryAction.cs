using System.Collections.Generic;
using Tactics.AI.Considerations;
using Tactics.AI.InfluenceMaps;
using Tactics.Entities;
using Tactics.Pathfinding;
using Tactics.Turns;
using Tactics.Utility;
using UnityEngine;

namespace Tactics.AI.Actions
{
	//todo: refactor to move to point on map, then choose a map type and highest/lowest.
	[CreateAssetMenu(menuName = "Tactics/Actions/Move Into Enemy Territory")]
	public class MoveIntoEnemyTerritoryAction : ScriptableAction
	{
		private NavNode _targetNode;
		[SerializeField] private int movementTurnsToConsider = 1;

		public override float ScoreAction(Agent agent, AIContext context)
		{
			//Finds the highest point in current movement range that is in enemy territory.

			//TODO: find lowest point in territory in our movement range.
			var map = InfluenceMap.Clone(context.TerritoryMap);
			map.MultiplyInfluence(agent.GetMovementRangeMap(movementTurnsToConsider));
			//todo remove non walkable.
			var lowest = map.GetLowestPosition();
			_targetNode = agent.CurrentNode.NavMap.GetNavNode(lowest.V2ToV3XZ());
			_agent = agent;
			//Action Veto: If there is no lowest point in our movement range, we can't move into enemy territory. We're surrounded by friends.
			if(map.GetValue(lowest)>=0)
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
				Debug.LogError("No target node, cant pathfind");
				return new DoNothingMove(_agent);
			}
			pathfinder.TryFindPath(_agent.CurrentNode, _targetNode, out var pathList);

			//no valid path
			if (pathList.Count == 0)
			{
				Debug.LogError("No path to target node or already there.");
				return new DoNothingMove(_agent);
			}

			//valid path...
			if (pathList.Count == 0)
			{
				Debug.LogError("No path to target node.");

				//We don't want to step on the enemy.
				//todo: have to determine how to move towards-but-not-on. goal is NEAR an enemy.
				return new DoNothingMove(_agent);
			}

			return new MoveAlongPath(_agent, pathList, _agent.range);
		}
	}
}