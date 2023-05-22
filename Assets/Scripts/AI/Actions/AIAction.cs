using Tactics.AI.InfluenceMaps;
using Tactics.Entities;
using Tactics.Turns;
using UnityEngine;

namespace Tactics.AI.Actions
{
	public abstract class AIAction
	{
		//score will be updated continuously as different agents consider the action.
		public float Score => _score;
		protected float _score;

		public void ScoreAction(Agent agent, AIContext context)
		{
			//score the action based on the context
			//this is where the action will be scored based on the influence maps
			//and the agent's current state
			//the action with the highest score will be chosen
			//testing
			_score = Random.value;
		}

		//affect influence map. This is a processing step for before move planning.
		public virtual void AffectInfluenceMap(Agent agent, InfluenceMap map, InfluenceMapType mapType)
		{
			//actions that move will add their range?
			
		}

		public abstract MoveBase GetMove();
	}
}