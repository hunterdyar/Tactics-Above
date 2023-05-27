using System;
using Tactics.AI.Blackboard;
using UnityEngine;

namespace Tactics.AI.Considerations
{
	[Serializable]
	public class Consideration
	{
		public BlackboardProperty input;
		public AnimationCurve evaluation;
		
		
	}
}