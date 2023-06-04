using System;

namespace Tactics.AI.Blackboard
{
	[AttributeUsage(validOn: System.AttributeTargets.Method | System.AttributeTargets.Field | System.AttributeTargets.Property)]

	public class BlackboardFloatElement : Attribute
	{
		public string Name;

		public delegate float GetFloatValueDelegate();

		public GetFloatValueDelegate GetValue;
	}
}