using System.Collections;
using Tactics.Entities;

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
	}
}