using System.Collections;

namespace Tactics.Turns
{
	public interface ITurnTaker
	{
		public void PrepareTurn();
		public IEnumerator TakeTurn();
	}
}