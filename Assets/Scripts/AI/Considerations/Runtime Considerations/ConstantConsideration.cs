using Tactics.AI.Actions;
using Tactics.Entities;

namespace Tactics.AI.Considerations
{
	public class ConstantConsideration : IConsideration
	{
		private float score;

		public ConstantConsideration(float score)
		{
			this.score = score;
		}

		public float ScoreConsideration(AIContext context)
		{
			return score;
		}
	}
}