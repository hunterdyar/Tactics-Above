namespace Tactics.AI.Blackboard.Constant_Propeties
{
	public class ConstantBlackboard
	{
		[BlackboardElement]
		public static float Zero()
		{
			return 0;
		}

		[BlackboardElement]
		public static float One()
		{
			return 1;
		}

		[BlackboardElement]
		public static float Half()
		{
			return 0.5f;
		}

		[BlackboardElement]
		public static float AlmostZero()
		{
			return 0.01f;
		}

		[BlackboardElement]
		public static float AlmostOne()
		{
			return 0.99f;
		}
	}
}