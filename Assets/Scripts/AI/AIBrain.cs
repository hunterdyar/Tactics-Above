using System.Collections.Generic;
using Tactics.AI.Actions;
using Tactics.Entities;
using Tactics.Turns;
using UnityEngine;

namespace Tactics.AI
{
	//UtilityAI brain
	public class AIBrain : MoveDecider
	{
		//AIBrain has a list of all possible actions.
		//These are all possible attacks, stored in the agent. (todo: for now?) 
		//They are also various tactical positioning options. 
		//To start, the personality of the aibrain will be determined by what actions are in the list. This might be good enough, but if not we can add personality stats that the Considerations can reference as input floats. 
		
		//To make a decision, first we need to get a list of all possible actions.
		//Then we score every action.
		//To score the action, it scores itself based on the considerations it has, which take the agent, faction, and AIContext as input to get knowledge about the world. the influence maps are part of the AIContext, and are precomputed models of understanding.
		//then we choose the highest scored action and set the agents NextMove with it.
 
		
		//We will choose one action each turn... supporting more? Like a Move and an attack? Sort of feels like we will need to do that... 

		public List<IAIAction> GetAllActions()
		{
			List<IAIAction> actions = new List<IAIAction>();
			foreach (var attack in _agent.Attacks)
			{
				foreach (var action in attack.GetAIActions(_agent))
				{
					actions.Add(action);
				}
			}

			return actions;
		}
	
		
		public float ScoreAction(IAIAction action, Agent agent, AIContext context)
		{
			float score = 0;
			var c = action.GetConsiderations();
			for (int i = 0; i < c.Count; i++)
			{
				float considerationScore = c[i].ScoreConsideration(action, agent, context);
				score *= considerationScore;
				if (score == 0)
				{
					action.Score = 0;
					return 0;
				}
			}
			 
			//averaging scheme by dave hill
			float originalScore = score;
			float modFactor = 1 - (1/c.Count);
			float makeupValue = (1-originalScore) * modFactor;
			action.Score = originalScore + (makeupValue*originalScore);

			return action.Score;
		}
		
		public override MoveBase DecideMove(AIContext context)
		{
			List<IAIAction> actions = GetAllActions();

			if (actions.Count == 0)
			{
				return new DoNothingMove(_agent);
			}

			foreach (var action in actions)
			{
				ScoreAction(action, _agent, context);
			}

			actions.Sort((a, b) => b.Score.CompareTo(a.Score));
			return actions[0].GetMove();
		}
	}
}