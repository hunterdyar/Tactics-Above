namespace Tactics.AI.Blackboard.Constant_Propeties
{
	public class ConstantBlackboard
	{
		[BlackboardElement]
		public static float Zero() => 0;

		[BlackboardElement] public static float One => 1;
		
		[BlackboardElement]
		public static float Half => 0.5f;
		
		[BlackboardElement]
		public static float AlmostZero => 0.01f;

		[BlackboardElement]
		public static float AlmostOne => 0.99f;


	}
}