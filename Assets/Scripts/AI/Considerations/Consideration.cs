using System;
using Tactics.AI.Actions;
using Tactics.AI.Blackboard;
using Tactics.Entities;
using Tactics.Turns;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tactics.AI.Considerations
{
	[Serializable]
	public class Consideration : IConsideration
	{
		public BlackboardProperty input;
		public AnimationCurve evaluation;
		
		public float ScoreConsideration(AIContext context)
		{
			return evaluation.Evaluate(input.GetFloat(new object[]{context}));
		}
	}
}