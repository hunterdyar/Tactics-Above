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
		
		//When I made this a partial class, I did think there would be more blackboard specific functions here...
	}
}