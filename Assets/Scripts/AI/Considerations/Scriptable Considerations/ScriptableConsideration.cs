using Tactics.AI.Actions;
using Tactics.AI.Blackboard;
using Tactics.Entities;
using UnityEngine;

namespace Tactics.AI.Considerations
{

	public abstract class ScriptableConsideration : ScriptableObject, IConsideration
	{
		public abstract float ScoreConsideration(AIContext context);
		
	}
}