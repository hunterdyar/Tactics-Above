using Tactics.Turns;
using UnityEngine;

namespace Tactics.AI
{
	public class DoNothingMoveDecider : MoveDecider
	{
		public override MoveBase DecideMove(AIContext context)
		{
			return new DoNothingMove(_agent);
		}
	}
}