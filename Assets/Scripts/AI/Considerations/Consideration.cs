using Tactics.AI.Actions;
using Tactics.Entities;
using UnityEngine;

namespace Tactics.AI.Considerations
{

	public abstract class Consideration : ScriptableObject
	{
		public abstract float ScoreConsideration(IAIAction action, Agent agent, AIContext context);
	}
}