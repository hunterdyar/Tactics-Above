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