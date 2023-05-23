using System.Collections.Generic;
using Tactics.AI.Considerations;
using Tactics.AI.InfluenceMaps;
using Tactics.Entities;
using Tactics.Turns;
using UnityEngine;

namespace Tactics.AI.Actions
{
	public abstract class ScriptableAction : ScriptableObject, IAIAction
	{
		public float Score { get; set; }
		[SerializeField] private List<ScriptableConsideration> _considerations;
		public virtual List<ScriptableConsideration> GetConsiderations()
		{
			return _considerations;
		}

		public virtual float ScoreAction(Agent agent, AIContext context)
		{
			float score = 1;
			var c = GetConsiderations();
			for (int i = 0; i < c.Count; i++)
			{
				float considerationScore = c[i].ScoreConsideration(this,agent, context);
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
		/// <summary>
		/// Will do nothing by default.
		/// </summary>
		public virtual void AffectInfluenceMap(Agent agent, ref InfluenceMap map, InfluenceMapType mapType)
		{
		}

		public abstract MoveBase GetMove();
	}
}