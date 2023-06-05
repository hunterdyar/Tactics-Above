using Tactics.AI.Actions;
using Tactics.AI.Blackboard;
using Tactics.Entities;

namespace Tactics.AI.InfluenceMaps
{
	public partial class InfluenceMap
	{
		[BlackboardElement]
		public float AgentPosition(IAIAction action, Agent agent, AIContext context)
		{
			return GetValue(agent.CurrentNode);
		}
	}
}