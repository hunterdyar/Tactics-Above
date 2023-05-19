using System.Collections;
using BTween;
using Tactics.Entities;
using Unity.VisualScripting;
using UnityEngine;

namespace Tactics.Turns
{
	public class MoveInDirection : MoveBase
	{
		private readonly Vector3Int _direction;
		private float _timeToMove = 0.2f;
		public MoveInDirection(Agent agent, Vector3Int direction) : base(agent)
		{
			_direction = direction;
		}

		public override bool CanStartMove()
		{
			var destination = _agent.CurrentNode.GridPosition + _direction;
			if(_agent.CurrentNode.NavMap.TryGetNavNode(destination,out var destNode))
			{
				//don't move agent.
				if (!_agent.AgentLayer.HasAnyEntity(destNode))
				{
					return destNode.Walkable;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}

		public override IEnumerator DoMove()
		{
			if (CanStartMove())
			{
				//copied and pasted here... hmmm...
				var destination = _agent.CurrentNode.GridPosition + _direction;
				if (_agent.CurrentNode.NavMap.TryGetNavNode(destination, out var destNode))
				{
					_agent.SetOnNode(destNode,false);
					var tween = _agent.transform.BMoveTo(destNode.WorldPosition, _timeToMove,Ease.Linear,true);
					while (tween.Running)
					{
						yield return null;
					}
				}
			}
			yield break;
		}
	}
}