using Tactics.AI.Actions;
using Tactics.AI.Blackboard;
using Tactics.Entities;

namespace Tactics.AI.InfluenceMaps
{
	public partial class InfluenceMap
	{
		[BlackboardElement]
		public float AgentPosition(AIContext context)
		{
			return GetValue(context.OperatingAgent.CurrentNode);
		}
	}
}