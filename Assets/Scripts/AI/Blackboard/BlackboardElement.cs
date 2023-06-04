using System;
using UnityEngine;

namespace Tactics.AI.Blackboard
{
	[System.Serializable]
	[AttributeUsage(validOn: System.AttributeTargets.Method | System.AttributeTargets.Field | System.AttributeTargets.Property)]
	public class BlackboardElement : Attribute
	{
		public string Name = "";

		public delegate object GetValueDelegate();

		public System.Type attribueType;
		public GetValueDelegate GetValue;

		public float GetValueAsFloat(float fallback = 0)
		{
			var o = GetValue.Invoke();

			if (o is float f)
			{
				return f;
			}

			if (o is int i)
			{
				return (float)i;
			}

			if (o is double d)
			{
				return (float)d;
			}
			
			if (o is bool b)
			{
				return b ? 1f : 0f;
			}
			
			//toString is slow. But is it as slow as doing the 5 above? Would be interesting to profile, maybe string-and-back isn't that slow.
			if(float.TryParse(o.ToString(), out var a))
			{
				return a;
			}

			return fallback;
		}
	}
}
