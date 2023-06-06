using System.Collections;
using Tactics.Entities;
using UnityEngine;

namespace Tactics.Turns
{
	public abstract class MoveBase
	{
		protected Agent _agent;

		protected MoveBase(Agent agent)
		{
			_agent = agent;
		}
		
		//todo this doesnt work here. Instead, we would get some modifier we would use for utilityAI. Like some "expected output" - can't, can, miss, etc.
		public virtual bool CanStartMove()
		{
			return true;
		}

		public virtual IEnumerator DoMove()
		{
			yield break;
		}
		//undo?
		public virtual void OnDrawGizmos()
		{
			
		}

		public void DebugDrawAttackLine(NavNode node)
		{
			Debug.DrawLine(_agent.CurrentNode.WorldPosition, node.WorldPosition, Color.red);
			Gizmos.color = Color.red;
			Gizmos.DrawWireCube(node.WorldPosition, Vector3.one);
			// Gizmos.DrawCube(node.WorldPosition, Vector3.one * 0.3f);
		}
	}
}