using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tactics.Turns;
using Unity.VisualScripting;
using UnityEngine;

namespace Tactics.Entities
{
	[CreateAssetMenu(fileName = "Agents", menuName = "Tactics/Entities/Agent Collection (entity map)", order = 0)]

	public class AgentCollection : EntityMap, ITurnTaker
	{
		//AgentCollection is just an entitymap, but they get a turn order, and all members in the list can take a turn!
		
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

		
		//A collection taking a turn is going one-by-one through agents for them to take their turn.
		public void PrepareTurn()
		{
			foreach (var agent in GetAgentsInOrder())
			{
				agent.PrepareTurn();
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