using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tactics.Entities;
using UnityEngine;

namespace Tactics.Turns
{
	public class MoveAlongPath : MoveBase
	{
		private List<NavNode> _path;
		private int _numMoves = 1;
		public MoveAlongPath(Agent agent, List<NavNode> path, int numMoves) : base(agent)
		{
			_path = path;
			_numMoves = numMoves;
		}

		public override IEnumerator DoMove()
		{
			int moves = Mathf.Min(_numMoves, _path.Count);
			for (int i = 0; i < moves; i++)
			{
				var direction = _path[i].GridPosition - _agent.CurrentNode.GridPosition;
				var subMove = new MoveInDirection(_agent, direction);
				if (subMove.CanStartMove())
				{
					yield return _agent.StartCoroutine(subMove.DoMove());
				}
				else
				{
					//don't keep trying
					break;
				}
			}
		}
	}
}