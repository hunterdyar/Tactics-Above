using UnityEngine;

namespace Tactics.AI
{
	public class AIBrain : MonoBehaviour
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
		
		
	}
}