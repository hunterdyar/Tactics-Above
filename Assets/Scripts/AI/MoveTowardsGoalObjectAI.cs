using Tactics.Entities;
using Tactics.Pathfinding;
using Tactics.Turns;
using UnityEngine;

namespace Tactics.AI
{
	public class MoveTowardsGoalObjectAI : MoveDecider
	{
		[SerializeField] private int movesPerTurn;
		private AStarPathfinder<NavNode> _pathfinder;
		public override void Initiate(Agent agent)
		{
			base.Initiate(agent);
			_pathfinder = new AStarPathfinder<NavNode>(agent.CurrentNode.NavMap, agent.IsNodeWalkable);
		}

		public override MoveBase DecideMove()
		{
			var goal = GameObject.Find("test goal").transform;
			if (CurrentNode.NavMap.TryGetNavNodeAtWorldPos(goal.position, out var goalNode))
			{
				//todo: if there is already an entity at the destination that we cannot walk on, then we won't be able to find a path, and cannot head 'towards' it.
				//So we need some kind of find closest valid destination for goal function, so agents can cluster around something.

				_pathfinder.TryFindPath(CurrentNode, goalNode, out var pathList);
				return new MoveAlongPath(_agent, pathList, movesPerTurn);
			}
			else
			{
				return new DoNothingMove(_agent);
			}
		}
	}
}