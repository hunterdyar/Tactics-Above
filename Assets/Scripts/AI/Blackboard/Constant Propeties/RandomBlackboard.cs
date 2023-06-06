using UnityEngine;

namespace Tactics.AI.Blackboard.Constant_Propeties
{
	public class RandomBlackboard
	{
		[BlackboardElement(Name = "Random 0-1")]
		public float Random01()
		{
			return Random.value;
		}
	}
}