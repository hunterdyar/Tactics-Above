using System.Collections.Generic;
using System.Linq;
using Attacks;
using Tactics.AI.Blackboard;
using Tactics.AI.InfluenceMaps;
using Tactics.Entities;
using Tactics.Pathfinding;
using Tactics.Turns;
using Tactics.Utility;
using UnityEngine;

namespace Tactics.AI.Actions
{
	[CreateAssetMenu(menuName = "Tactics/Actions/Move To Attacking Position")]

	public class MoveToAttackingPosition : ScriptableAction
	{
		private NavNode _targetNode;
		[SerializeField] private int movementTurnsToConsider = 1;

		private int numAttacksForBestMove;
		public override float ScoreAction(Agent agent, AIContext context)
		{
			//reset
			numAttacksForBestMove = 0;
			//set
			_agent = agent;
			context.SetAction(this);
			context.SetOperatingAgent(agent);
			
			//score

			//flood filling our movement, loop through all attack actions and check if it would hit an enemy. If it does, add it to a list of options with the action, path, and so on.
			//sort this path by some serialized properties, like simulated damage dealing or safety of position.
			//choose best one.
			
			//todo: move this to the GetOptions function, doing all cache checks within the calculator.
			_agent.AttackOptionCalculator.CalculateMovementToAttackOptions(movementTurnsToConsider);

			var options = _agent.AttackOptionCalculator.GetOptionsByLocation();
			//Find the best one. Use our own serialized setup for sorting, so we can have multiple versions of this movement action finding different target types.
			if (options.Count == 0)
			{
				//none of our movement will get us to an attack position. This movement action is no good.
				Score = 0;
				return Score;
			}
			//For now, lets sort by what one will 1. hit the most targets, then 2: deal the most damage. 
			
			//many could tie, we can also sort by how intrusive we would be to an enemies territory.
			List<NavNode> bestOptions = new List<NavNode>();
			int targetCount = 0;
			
			//we could early exit here if there is only 1 option, but if so this is barely slower?
			
			//Create a list of all movement options that would the most attacks. If all options hit one enemy, this will be a list of all keys.
			
			//todo i messed this up, im counting number of attacks from position, not number enemies. Which is still sort of usable; works when attacks are all one node targets.
			//one attack hitting multiple enemies and two attacks each hitting one...
			
			//I think we should do all the sorting in the calculator and then expose a bunch of attacks like "best attack one move away" and "biggest damage attack".
			//getting into like, table and query syntax. which i obviously don't want to do.
			
			//im not worrying about remembering what attack we want to do. New turn new me, recalculate all options. (although only calculating once will be good).

			foreach (var option in options)
			{
				var c = option.Value.Count;
				if (c == targetCount)
				{
					bestOptions.Add(option.Key);
				}else if (c > targetCount)
				{
					targetCount = c;
					bestOptions.Clear();
					bestOptions.Add(option.Key);
				}
			}

			numAttacksForBestMove = targetCount;//again, this is wrong
			
			//early exit if we only have one best option so far.
			
			if (bestOptions.Count == 1)
			{
				//no need to keep sorting
				_targetNode = bestOptions[0];
				return base.ScoreAction(agent, context);
			}
			
			//Now sort the attacks. Here's a conundrum: what's better, 1 damage this turn or 3 damage next turn? Well, enemies move, so for now let's just sort with closer steps being better.
			var attacks = options.Where(x => bestOptions.Contains(x.Key)).SelectMany(x => x.Value).ToList();
			attacks = SortAttacks(attacks);

			_targetNode = attacks[0].destination;

			//the agent could have multiple of this action, and we sort the list differently... So I think if we make a new monobehaviour that calculates (and caches) options. 

			return base.ScoreAction(agent, context);
		}

		private List<MoveToAttackOption> SortAttacks(List<MoveToAttackOption> attacks)
		{
			
			return attacks.OrderBy(x =>
			{
				//i WANT to lost the fraction. I care about turns, not steps.
				float z = (float)(int)(x.stepsStillToMove / _agent.range);
				//then we will sort by damage
				z+=x.GetSimulatedDamageDealt() * 0.1f;
				//then we will sort by territory coverage. todo.
				return z;
			}).ToList();
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

		[BlackboardElement]
		public int GetNumberAttacksBestMoveHas()
		{
			return numAttacksForBestMove;
		}
	}
}