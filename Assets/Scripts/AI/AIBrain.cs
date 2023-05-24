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
		//The agent will then execute the move, and the turn will end.
		
		//We will choose one action each turn... supporting more? Like a Move and an attack? Sort of feels like we will need to do that... 

		[SerializeField] private ScriptableAction[] _actions;
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

			foreach (var sa in _actions)
			{
				actions.Add(sa);
			}

			return actions;
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
				action.ScoreAction(_agent, context);
			}

			actions.Sort((a, b) => b.Score.CompareTo(a.Score));
			if (actions[0].Score == 0)
			{
				Debug.Log($"All actions were 0. {gameObject.name} could not decide. Doing nothing.");
				return new DoNothingMove(_agent);
			}
			Debug.Log($"{gameObject.name} decided {actions[0].GetType().Name}");
			return actions[0].GetMove();
		}
	}
}