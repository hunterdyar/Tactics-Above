﻿using Tactics.AI.Actions;
using Tactics.Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tactics.AI.Considerations
{	
	[CreateAssetMenu(fileName = "WouldAttackEntityConsideration", menuName = "Tactics/Considerations/Would Attack Entity Consideration")]
	public class WouldAttackEntityScriptableConsideration : ScriptableConsideration
	{
		public FactionContext factionContext;
		[Tooltip("If true, will return 0 if the entity is hit. If false, will return 1 if the entity would be hit.")]
		public bool invert;

		[FormerlySerializedAs("modifier")] [Range(0, 1)] public float trueValue = 1; 
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
							return 1-trueValue;
						}
						return trueValue;
					}
				}
			}
			
			if(invert)
			{
				return trueValue;
			}
			return 1-trueValue;
		}
	}
}