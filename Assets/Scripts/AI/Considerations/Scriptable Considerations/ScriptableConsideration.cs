using Tactics.AI.Actions;
using Tactics.AI.Blackboard;
using Tactics.Entities;
using UnityEngine;

namespace Tactics.AI.Considerations
{

	public abstract class ScriptableConsideration : ScriptableObject, IConsideration
	{
		public BlackboardProperty test;
		public abstract float ScoreConsideration(IAIAction action, Agent agent, AIContext context);
		
		[ContextMenu("Test blackboard")]
		void TestBlackboard()
		{
			test.FindElements();

		}
	}
}