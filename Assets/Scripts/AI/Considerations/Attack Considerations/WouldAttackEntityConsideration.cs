using Tactics.AI.Actions;
using Tactics.Entities;
using UnityEngine;

namespace Tactics.AI.Considerations
{	
	[CreateAssetMenu(fileName = "WouldAttackEntityConsideration", menuName = "Tactics/Considerations/Would Attack Entity Consideration")]
	public class WouldAttackEntityConsideration : Consideration
	{
		public FactionContext factionContext;
		[Tooltip("If true, will return 0 if the entity is hit. If false, will return 1 if the entity would be hit.")]
		public bool invert;
		public override float ScoreConsideration(IAIAction action, Agent agent, AIContext context)
		{
			if (action is AttackAIAction attackAction)
			{
				var targetNodes = attackAction.TargetNodes;
				foreach (var node in targetNodes)
				{
					if (context.GetFactionFromContext(factionContext).HasAnyEntity(node))
					{
						if(invert)
						{
							return 0f;
						}
						return 1f;
					}
				}
			}
			
			if(invert)
			{
				return 1f;
			}
			return 0f;
		}
	}
}