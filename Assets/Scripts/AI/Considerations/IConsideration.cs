using Tactics.AI.Actions;
using Tactics.Entities;

namespace Tactics.AI.Considerations
{
	public interface IConsideration
	{
		public  float ScoreConsideration(IAIAction action, Agent agent, AIContext context);
	}
}