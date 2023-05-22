using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tactics.AI;
using Tactics.AI.InfluenceMaps;
using Tactics.Turns;
using UnityEngine;

namespace Tactics.Entities
{
	[CreateAssetMenu(fileName = "Agents", menuName = "Tactics/Entities/Agent Collection (entity map)", order = 0)]

	public class Faction : EntityMap
	{
		[Header("Debug")]
		public Gradient TerritoryGradient;

		private Faction[] _enemies;
		//AgentCollection is just an entity map, but they get a turn order, and all members in the list can take a turn!
		public InfluenceMap TerritoryMap => _territoryMap;
		public InfluenceMap AttackMap => _attackMap;

		private InfluenceMap _territoryMap;
		private InfluenceMap _attackMap;
		
		public AIContext AIContext => _currentAIContext;
		private AIContext _currentAIContext;
		public List<Agent> GetAgentsInOrder()
		{
			var list = _entities.Values.Cast<Agent>().ToList();
			list.Sort(SortAgents);
			return list;
		}
		
		//void PrepareTurn
		//IEnumerator TakeTurn

		private int SortAgents(Agent x, Agent y)
		{
			//switch case sorting types for whatever options.
			int c = x.CurrentNode.GridPosition.x - y.CurrentNode.GridPosition.x;
			if (c == 0)
			{
				return	x.CurrentNode.GridPosition.y - y.CurrentNode.GridPosition.y;
			}
			else
			{
				return c;
			}
		}

		public bool TryGetClosestAgentInMap(NavNode node, out Agent agent)
		{
			//todo: Pathfinding?
			if (_entities.Count == 0)
			{
				agent = null;
				return false;
			}

			var closest = _entities.Keys.OrderBy(x => Vector3Int.Distance(node.GridPosition, x.GridPosition)).First();
			if (_entities[closest] is Agent a)
			{
				agent = a;
				return true;
			}

			agent = null;
			return false;
		}
		
		public void PrepareKnowledge(Faction[] enemies)
		{
			_enemies = enemies;//cache
			_territoryMap = InfluenceMap.New(NavMap,0);
			_attackMap = InfluenceMap.New(NavMap,0);
			foreach (var agent in GetAgentsInOrder())
			{
				_territoryMap.AddInfluence(agent.GetTerritoryInfluence());
				_attackMap.AddInfluence(agent.GetInfluenceFromAttacks(InfluenceMapType.Attack));
			}
		}
		//A collection taking a turn is going one-by-one through agents for them to take their turn.
		public void PrepareTurn()
		{
			//context clones the maps we make here, and we modify it as the agents make their decisions, so following agents can compensate.
			_currentAIContext = new AIContext(this, _enemies);
			foreach (var agent in GetAgentsInOrder())
			{
				agent.PrepareTurn(_currentAIContext);
			}
		}

		public IEnumerator TakeTurn()
		{
			foreach (var agent in GetAgentsInOrder())
			{
				yield return agent.StartCoroutine(agent.TakeTurn());
			}
		}
	}
}