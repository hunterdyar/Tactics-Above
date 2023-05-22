using Tactics.Entities;
using Tactics.Turns;
using UnityEngine;

namespace Tactics.AI
{
	public abstract class MoveDecider : MonoBehaviour
	{
		protected Agent _agent;
		protected NavNode CurrentNode => _agent.CurrentNode;
		protected NavMap NavMap => CurrentNode.NavMap;
		public virtual void Initiate(Agent agent)
		{
			_agent = agent;
		}

		public abstract MoveBase DecideMove(AIContext context);
	}
}