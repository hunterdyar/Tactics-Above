using Tactics.AI.Actions;
using Tactics.Entities;
using UnityEngine;

namespace Tactics.AI.Considerations
{
	[CreateAssetMenu(fileName = "constant", menuName = "Tactics/Considerations/Constant Consideration")]
	public class TuningConsideration : ScriptableConsideration
	{
		[Range(0,1f)]
		public float constantScore;
		public override float ScoreConsideration(AIContext context)
		{
			return constantScore;
		}
	}
}