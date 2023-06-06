using System.Collections.Generic;
using Tactics.AI.Considerations;
using Tactics.AI.InfluenceMaps;
using Tactics.Entities;
using Tactics.Turns;
using UnityEngine;

namespace Tactics.AI.Actions
{
	public interface IAIAction 
	{
		//score will be updated continuously as different agents consider the action.
		public float Score { get; set; }
		
		public List<IConsideration> GetConsiderations();

		public virtual float ScoreAction(Agent agent, AIContext context){
			float score = 1;
			context.SetOperatingAgent(agent);
			context.SetAction(this);
			var c = GetConsiderations();
			for (int i = 0; i < c.Count; i++)
			{
				float considerationScore = c[i].ScoreConsideration(context);
				score *= considerationScore;
				if (score == 0)
				{
					Score = 0;
					return 0;
				}
			}

			//averaging scheme by dave hill
			float originalScore = score;
			float modFactor = 1 - (1 / c.Count);
			float makeupValue = (1 - originalScore) * modFactor;
			Score = originalScore + (makeupValue * originalScore);

			return Score;
		}
		//affect influence map. This is a processing step for before move planning.
		public abstract void AffectInfluenceMap(Agent agent, ref InfluenceMap map, InfluenceMapType mapType);

		public abstract MoveBase GetMove();
	}
}