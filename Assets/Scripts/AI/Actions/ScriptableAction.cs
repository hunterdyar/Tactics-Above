using System.Collections.Generic;
using System.Linq;
using Tactics.AI.Blackboard;
using Tactics.AI.Considerations;
using Tactics.AI.InfluenceMaps;
using Tactics.Entities;
using Tactics.Turns;
using Unity.VisualScripting;
using UnityEngine;

namespace Tactics.AI.Actions
{
	public abstract class ScriptableAction : ScriptableObject, IAIAction
	{
		private AIContext _context;
		protected Agent _agent;
		
		public Consideration[] TestConsiderations;
		public float Score { get; set; }
		[SerializeField] private List<ScriptableConsideration> _considerations;
		public virtual List<IConsideration> GetConsiderations()
		{
			foreach (var tc in TestConsiderations)
			{
				tc.input.Init();
			}
			return _considerations.ConvertAll(x => (IConsideration)x).Union(TestConsiderations.ToList().ConvertAll(x=>(IConsideration)x)).ToList();
		}

		public virtual float ScoreAction(Agent agent, AIContext context)
		{
			//Cache these because the serialized blackboard properties go through functions with [blackboardElement] attributes, which we can't embed in the function.
			_context = context;
			_agent = agent;
			
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

		[BlackboardElement(Name = "AI Context")]
		public AIContext GetLastAIContext()
		{
			return _context;
		}

		[BlackboardElement(Name = "Agent")]
		public Agent GetConsideringAgent()
		{
			return _agent;
		}

		[BlackboardElement]
		public float GetRandomNumber()
		{
			return Random.value;
		}

		[BlackboardElement]
		public int ConstantZero()
		{
			return 0;
		}
	}
}