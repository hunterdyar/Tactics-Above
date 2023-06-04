using System;
using Tactics.AI.Blackboard;
using Tactics.Entities;
using Tactics.Turns;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tactics.AI.Considerations
{
	[Serializable]
	public class Consideration
	{
		public BlackboardProperty input;
		public AnimationCurve evaluation;
		
	}
}